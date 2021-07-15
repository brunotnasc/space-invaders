using System;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(HomingProjectileController))]
    public class TargetLock : MonoBehaviour
    {
        [SerializeField]
        private GameObject targetMarker;
        private HomingProjectileController projectile;

        private void Awake()
        {
            projectile = GetComponent<HomingProjectileController>();
            projectile.TargetAcquired += OnTargetAcquiredEventHandler;
            projectile.TargetLost += OnTargetLostEventHandler;
            projectile.Despawned += OnProjectileDespawnedEventHandler;
            targetMarker.SetActive(false);
        }

        private void OnTargetAcquiredEventHandler(object sender, EntityController target)
        {
            targetMarker.transform.SetParent(target.transform, false);
            targetMarker.SetActive(true);
        }

        private void OnTargetLostEventHandler(object sender, EntityController target)
        {
            targetMarker.transform.SetParent(transform, false);
            targetMarker.SetActive(false);
        }

        private void OnProjectileDespawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is HomingProjectileController)
            {
                targetMarker.transform.SetParent(transform, false);
                targetMarker.SetActive(false);
            }
        }
    }
}