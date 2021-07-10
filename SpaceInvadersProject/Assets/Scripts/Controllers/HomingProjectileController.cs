using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class HomingProjectileController : ProjectileController
    {
        public event EventHandler<EntityController> TargetAcquired;
        public event EventHandler<EntityController> TargetLost;

        public EntityController Target { get; private set; }

        [SerializeField, Range(10f, 200f)]
        private float guidanceStrength = 10f;

        public override void Spawn(Vector2 position, Vector2 direction)
        {
            base.Spawn(position, direction);
            Target = null;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsExploding)
            {
                return;
            }

            if (Target == null)
            {
                Target = FindInvaderTarget();
            }

            GuideProjectile();
        }

        private void GuideProjectile()
        {
            if (Target != null)
            {
                if (Target.IsExploding)
                {
                    OnTargetLost();
                    return;
                }

                Vector2 forward = transform.up.normalized;
                Vector2 direction = (Target.RBPosition - RBPosition).normalized;
                float angle = Vector2.SignedAngle(forward, direction);

                rb2D.velocity = rb2D.velocity.magnitude * forward;
                rb2D.AddForce(direction * Mathf.Max(1f, guidanceStrength * 0.5f));
                rb2D.AddTorque(angle * guidanceStrength);
            }
        }

        private InvaderController FindInvaderTarget()
        {
            InvaderController closestTarget = null;
            float closestTargetDistance = float.PositiveInfinity;

            foreach (InvaderController invader in gameManager.Invaders)
            {
                if (invader.IsExploding || !invader.isActiveAndEnabled)
                {
                    continue;
                }

                float distanceToTarget = Vector2.Distance(RBPosition, invader.RBPosition);
                bool isCloser = distanceToTarget < closestTargetDistance;
                if (isCloser)
                {
                    closestTarget = invader;
                    closestTargetDistance = distanceToTarget;
                }
            }

            if (closestTarget != null)
            {
                TargetAcquired?.Invoke(this, closestTarget);
            }

            return closestTarget;
        }

        private void OnTargetLost()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("Reacquiring target");
            SetColor(Color.yellow);
#endif
            TargetLost?.Invoke(this, Target);
            Target = null;
        }
    }
}