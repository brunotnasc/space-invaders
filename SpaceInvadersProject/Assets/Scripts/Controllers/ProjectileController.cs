using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(Animator))]
    public class ProjectileController : EntityController
    {
        [SerializeField]
        protected LayerMask doDamageOnCollision;
        [SerializeField]
        private int damage;
        [SerializeField]
        private float speed;

        private Animator animator;
        private Vector2 previousPosition;

        public override void Spawn(Vector2 position, Vector2 direction)
        {
            base.Spawn(position, direction);

            if (animator != null)
            {
                animator.enabled = true;
            }

            rb2D.transform.up = direction.normalized;
            Vector2 velocity = direction * speed;
            rb2D.velocity = velocity;
            previousPosition = Vector2.positiveInfinity;
        }

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected override void OnCollision(GameObject other)
        {
            base.OnCollision(other);

            int layer = 1 << other.layer;
            IDestroyable destroyable = other.GetComponent<IDestroyable>();
            bool isDamageable = destroyable != null && (layer & doDamageOnCollision.value) != 0;
            if (isDamageable)
            {
                destroyable.TakeDamage(damage);
            }
        }

        protected override IEnumerator ExplosionCoroutine()
        {
            if (animator != null)
            {
                animator.enabled = false;
            }
            yield return base.ExplosionCoroutine();
        }

        protected virtual void FixedUpdate()
        {
            if (IsExploding)
            {
                return;
            }

            if (Vector2.Distance(previousPosition, RBPosition) < 2f)
            {
                Destroy();
            }
            previousPosition = RBPosition;
        }
    }
}