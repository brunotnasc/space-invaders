using System;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(HomingProjectileController))]
    public class TargetPainter : MonoBehaviour
    {
        [SerializeField]
        private Color targetColor = Color.red;

        private HomingProjectileController projectile;

        private void Awake()
        {
            projectile = GetComponent<HomingProjectileController>();
            projectile.TargetAcquired += OnTargetAcquiredEventHandler;
            projectile.TargetLost += OnTargetLostEventHandler;
            projectile.Despawned += OnProjectileDespawnedEventHandler;
        }

        private void OnTargetAcquiredEventHandler(object sender, EntityController target)
        {
            target.SetColor(targetColor);
        }

        private void OnTargetLostEventHandler(object sender, EntityController target)
        {
            target.ResetColor();
        }

        private void OnProjectileDespawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is HomingProjectileController projectile)
            {
                if (projectile.Target != null)
                {
                    projectile.Target.ResetColor();
                }
            }
        }
    }
}