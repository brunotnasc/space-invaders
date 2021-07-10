using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    public class ProjectilePoolManager : MonoBehaviour
    {
        /// <summary>
        /// Pool auto-expansível responsável por instanciar e armazenar projéteis.
        /// </summary>
        private class ProjectilePool : Stack<ProjectileController>
        {
            private readonly GameObject projectilePrefab;
            private readonly Transform container;

            public ProjectilePool(int capacity, GameObject projectilePrefab, Transform container) : base(capacity)
            {
                this.projectilePrefab = projectilePrefab;
                this.container = container;
                Fill(capacity);
            }

            public new ProjectileController Pop()
            {
                if (Count == 0)
                {
                    Fill(1);
                }
                return base.Pop();
            }

            private void Fill(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    ProjectileController projectile = InstantiateProjectile(projectilePrefab);
                    projectile.Despawned += OnProjectileDespawnedEventHandler;
                    Push(projectile);
                }
            }

            private ProjectileController InstantiateProjectile(GameObject prefab)
            {
                GameObject go = Instantiate(prefab, container);
                ProjectileController projectile = go.GetComponent<ProjectileController>();
                go.SetActive(false);
                return projectile;
            }

            private void OnProjectileDespawnedEventHandler(object sender, EventArgs e)
            {
                if (sender is ProjectileController projectile)
                {
                    Push(projectile);
                }
            }
        }

        [SerializeField, Header("Projectile Prefabs")]
        private GameObject playerPrimaryProjectilePrefab;
        [SerializeField]
        private GameObject playerSecondaryProjectilePrefab;
        [SerializeField]
        private GameObject[] invaderProjectilePrefabs;

        private ProjectilePool playerPrimaryPool;
        private ProjectilePool playerSecondaryPool;
        private ProjectilePool[] invaderPool;

        public ProjectileController GetPlayerPrimaryProjectile()
        {
            return playerPrimaryPool.Pop();
        }

        public ProjectileController GetPlayerSecondaryProjectile()
        {
            return playerSecondaryPool.Pop();
        }

        public ProjectileController GetRandomEnemyProjectile()
        {
            int projectileIndex = UnityEngine.Random.Range(0, invaderProjectilePrefabs.Length);
            return invaderPool[projectileIndex].Pop();
        }

        private void Awake()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            playerPrimaryPool = new ProjectilePool(1, playerPrimaryProjectilePrefab, transform);
            playerSecondaryPool = new ProjectilePool(4, playerSecondaryProjectilePrefab, transform);

            int invaderProjectileTypes = invaderProjectilePrefabs.Length;
            invaderPool = new ProjectilePool[invaderProjectileTypes];
            for (int i = 0; i < invaderProjectileTypes; i++)
            {
                GameObject prefab = invaderProjectilePrefabs[i];
                invaderPool[i] = new ProjectilePool(3, prefab, transform);
            }
        }
    }
}