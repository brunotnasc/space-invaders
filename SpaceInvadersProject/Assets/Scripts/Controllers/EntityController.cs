using System;
using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public abstract class EntityController : MonoBehaviour, IDestroyable, ISpawnable
    {
        public event EventHandler Destroyed;
        public event EventHandler Spawned;
        public event EventHandler Despawned;

        public Vector2 RBPosition => rb2D.position;
        public bool IsExploding { get; protected set; }
        public int Health { get; protected set; }

        [SerializeField]
        protected int maxHP;
        [SerializeField]
        protected float explosionDuration;
        [SerializeField]
        protected Sprite explosionSprite;
        [SerializeField]
        protected LayerMask destroyOnCollision;
        [SerializeField]
        protected LayerMask despawnOnCollision;

        protected SpriteRenderer spriteRenderer;
        protected Sprite initialSprite;
        protected Rigidbody2D rb2D;
        protected GameManager gameManager;
        protected Coroutine explosionCoroutine;

        public virtual void Spawn(Vector2 position, Vector2 direction)
        {
            ResetController();
            transform.position = position;
            gameObject.SetActive(true);
            Spawned?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Despawn()
        {
            gameObject.SetActive(false);
            Despawned?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Destroy()
        {
            if (explosionCoroutine == null && isActiveAndEnabled)
            {
                rb2D.velocity = Vector2.zero;
                explosionCoroutine = StartCoroutine(ExplosionCoroutine());
            }
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Destroy();
            }
        }

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            initialSprite = spriteRenderer.sprite;
            rb2D = GetComponent<Rigidbody2D>();
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            OnCollision(collision.gameObject);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollision(collision.gameObject);
        }

        protected virtual void OnCollision(GameObject other)
        {
            int layer = 1 << other.layer;
            if ((layer & destroyOnCollision.value) != 0)
            {
                Destroy();
            }
            else if ((layer & despawnOnCollision.value) != 0)
            {
                Despawn();
            }
        }

        protected virtual IEnumerator ExplosionCoroutine()
        {
            IsExploding = true;
            spriteRenderer.sprite = explosionSprite;
            yield return new WaitForSeconds(explosionDuration);
            Destroyed?.Invoke(this, EventArgs.Empty);
            yield return null;
            Despawn();
        }

        protected virtual void ResetController()
        {
            Health = maxHP;
            spriteRenderer.sprite = initialSprite;
            if (explosionCoroutine != null)
            {
                StopCoroutine(explosionCoroutine);
                explosionCoroutine = null;
            }
            IsExploding = false;
        }
    }
}