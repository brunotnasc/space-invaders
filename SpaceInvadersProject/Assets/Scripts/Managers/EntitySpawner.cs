using System;
using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public abstract class EntitySpawner<TSpawnable> : MonoBehaviour where TSpawnable : ISpawnable
    {
        [SerializeField]
        protected TSpawnable spawnableElement;
        [SerializeField]
        protected Transform[] spawnPoints;
        [SerializeField]
        protected float spawnDelay = 3f;
        [SerializeField]
        protected bool autoRespawn = false;

        protected Coroutine spawnCoroutine;
        private GameManager gameManager;

        /// <summary>
        /// Spawn the <typeparamref name="TSpawnable"/> at one of its predefined 
        /// respawn points after a predefined amount of time.
        /// </summary>
        public virtual void Spawn()
        {
            Spawn(spawnDelay);
        }

        /// <summary>
        /// Spawn the <typeparamref name="TSpawnable"/> at one of its predefined 
        /// respawn points after <paramref name="delay"/> seconds.
        /// </summary>
        /// <param name="delay">Time to respawn.</param>
        public virtual void Spawn(float delay)
        {
            if (spawnCoroutine == null && isActiveAndEnabled)
            {
                IEnumerator routine = SpawnCoroutine(
                    spawnableElement,
                    GetSpawnPosition(),
                    GetSpawnDirection(),
                    delay);
                spawnCoroutine = StartCoroutine(routine);
            }
        }

        protected abstract Vector2 GetSpawnPosition();

        protected abstract Vector2 GetSpawnDirection();

        protected virtual void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameManager.LevelFinished += OnLevelFinishedEventHandler;
            gameManager.GameOver += OnGameOverEventHandler;
            spawnableElement.Despawned += OnElementDespawnedEventHandler;
        }

        private IEnumerator SpawnCoroutine(ISpawnable spawnable, Vector2 position, Vector2 direction, float delay)
        {
            yield return new WaitForSeconds(delay);
            spawnable.Spawn(position, direction);
            spawnCoroutine = null;
        }

        private void OnElementDespawnedEventHandler(object sender, EventArgs e)
        {
            if (autoRespawn)
            {
                Spawn();
            }
        }

        private void OnLevelFinishedEventHandler(object sender, EventArgs e)
        {
            if (sender is GameManager)
            {
                if (spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                    spawnCoroutine = null;
                }
            }
        }

        private void OnGameOverEventHandler(object sender, EventArgs e)
        {
            if (sender is GameManager)
            {
                if (spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                    spawnCoroutine = null;
                }
            }
        }
    }
}