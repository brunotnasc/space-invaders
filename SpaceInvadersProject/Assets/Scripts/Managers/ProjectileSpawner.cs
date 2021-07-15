using System;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ProjectilePoolManager))]
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField, Range(1, 5)]
        private int maxPrimaryProjectilesActive = 1;
        [SerializeField, Range(1, 4)]
        private int maxSecondaryProjectilesActive = 4;
        [SerializeField, Range(1, 10)]
        private int maxInvaderProjectilesActive = 3;
        [SerializeField, Range(1, 100), Space]
        private int gainSecondaryEveryNShots = 10;
        [SerializeField, Space]
        private PlayerResourceManager secondaryResourceManager;

        private ProjectilePoolManager pool;
        private int primaryProjectileActiveCount;
        private int secondaryProjectileActiveCount;
        private int invaderProjectileActiveCount;
        private int primaryProjectileFireCount;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        public void SetMaxProjectileAndMissileGain(
            int maxPrimaryProjectilesActive,
            int maxSecondaryProjectilesActive,
            int gainSecondaryEveryNShots)
        {
            this.maxPrimaryProjectilesActive = maxPrimaryProjectilesActive;
            this.maxSecondaryProjectilesActive = maxSecondaryProjectilesActive;
            this.gainSecondaryEveryNShots = gainSecondaryEveryNShots;
        }
#endif

        public void Register(IShooter shooter)
        {
            if (shooter is PlayerController)
            {
                shooter.PrimaryFired += OnPlayerFiredPrimaryEventHandler;
                shooter.SecondaryFired += OnPlayerFiredSecondaryEventHandler;
            }
            else
            {
                shooter.PrimaryFired += OnInvaderFiredEventHandler;
            }
        }

        private void Awake()
        {
            pool = GetComponent<ProjectilePoolManager>();
        }

        private void OnPlayerFiredPrimaryEventHandler(object sender, Vector2 position)
        {
            if (primaryProjectileActiveCount >= maxPrimaryProjectilesActive)
            {
                return;
            }

            ProjectileController projectile = pool.GetPlayerPrimaryProjectile();
            projectile.Despawned += OnPlayerPrimaryProjectileDespawnedEventHandler;

            Vector2 direction = Vector2.up;
            Vector2 offset = direction * 40f;
            FireProjectile(projectile, position + offset, direction);

            primaryProjectileActiveCount++;
            primaryProjectileFireCount++;

            if (primaryProjectileFireCount >= gainSecondaryEveryNShots)
            {
                primaryProjectileFireCount = 0;
                secondaryResourceManager.AddResourceUnit();
            }
        }

        private void OnPlayerFiredSecondaryEventHandler(object sender, Vector2 position)
        {
            if (secondaryProjectileActiveCount >= maxSecondaryProjectilesActive)
            {
                return;
            }

            if (secondaryResourceManager.Consume() == false)
            {
                return;
            }

            ProjectileController projectile = pool.GetPlayerSecondaryProjectile();
            projectile.Despawned += OnPlayerSecondaryProjectileDespawnedEventHandler;

            Vector2 direction = Vector2.up;
            Vector2 offset = direction * 40f;
            FireProjectile(projectile, position + offset, direction);

            secondaryProjectileActiveCount++;
        }

        private void OnInvaderFiredEventHandler(object sender, Vector2 position)
        {
            if (invaderProjectileActiveCount >= maxInvaderProjectilesActive)
            {
                return;
            }

            ProjectileController projectile = pool.GetRandomEnemyProjectile();
            projectile.Despawned += OnInvaderProjectileDespawnedEventHandler;

            Vector2 direction = Vector2.down;
            Vector2 offset = direction * 40;
            FireProjectile(projectile, position + offset, direction);

            invaderProjectileActiveCount++;
        }

        private void FireProjectile(ProjectileController projectile, Vector2 position, Vector2 direction)
        {
            projectile.Spawn(position, direction);
        }

        private void OnPlayerPrimaryProjectileDespawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is ProjectileController projectile)
            {
                primaryProjectileActiveCount--;
                projectile.Despawned -= OnPlayerPrimaryProjectileDespawnedEventHandler;
            }
        }

        private void OnPlayerSecondaryProjectileDespawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is ProjectileController projectile)
            {
                secondaryProjectileActiveCount--;
                projectile.Despawned -= OnPlayerSecondaryProjectileDespawnedEventHandler;
            }
        }

        private void OnInvaderProjectileDespawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is ProjectileController projectile)
            {
                invaderProjectileActiveCount--;
                projectile.Despawned -= OnInvaderProjectileDespawnedEventHandler;
            }
        }
    }
}