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
        protected Transform spawnPoint;
        [SerializeField]
        protected bool autoRespawn = false;
        [SerializeField]
        protected float respawnDelay = 3f;

        private Coroutine respawnCoroutine;
        private GameManager gameManager;

        public virtual void Respawn()
        {
            Respawn(respawnDelay);
        }

        public virtual void Respawn(float delay)
        {
            if (respawnCoroutine == null && isActiveAndEnabled)
            {
                IEnumerator routine = RespawnCoroutine(
                    spawnableElement,
                    GetPosition(),
                    GetDirection(),
                    delay);
                respawnCoroutine = StartCoroutine(routine);
            }
        }

        public virtual Vector2 GetPosition()
        {
            return spawnPoint.position;
        }

        public abstract Vector2 GetDirection();

        protected virtual void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameManager.LevelFinished += OnLevelFinishedEventHandler;
            gameManager.GameOver += OnGameOverEventHandler;
            spawnableElement.Despawned += OnElementDespawnedEventHandler;
        }

        private IEnumerator RespawnCoroutine(ISpawnable spawnable, Vector2 position, Vector2 direction, float delay)
        {
            yield return new WaitForSeconds(delay);
            spawnable.Spawn(position, direction);
            respawnCoroutine = null;
        }

        private void OnElementDespawnedEventHandler(object sender, EventArgs e)
        {
            if (autoRespawn)
            {
                Respawn();
            }
        }

        private void OnLevelFinishedEventHandler(object sender, EventArgs e)
        {
            if (sender is GameManager)
            {
                if (respawnCoroutine != null)
                {
                    StopCoroutine(respawnCoroutine);
                    respawnCoroutine = null;
                }
            }
        }

        private void OnGameOverEventHandler(object sender, EventArgs e)
        {
            if (sender is GameManager)
            {
                if (respawnCoroutine != null)
                {
                    StopCoroutine(respawnCoroutine);
                    respawnCoroutine = null;
                }
            }
        }
    }
}