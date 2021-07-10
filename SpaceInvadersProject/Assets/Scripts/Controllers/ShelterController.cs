using System;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public class ShelterController : MonoBehaviour, IDestroyable, ISpawnable
    {
        public event EventHandler Destroyed;
        public event EventHandler Spawned;
        public event EventHandler Despawned;

        [SerializeField]
        private int healthPoints;
        [SerializeField]
        private LayerMask destroyOnCollision;
        [SerializeField, Space]
        private Sprite[] sprites;

        private int currentHP;
        private SpriteRenderer spriteRenderer;
        private Sprite initialSprite;
        private GameManager gameManager;

        public virtual void Spawn(Vector2 position, Vector2 direction)
        {
            currentHP = healthPoints;
            spriteRenderer.sprite = initialSprite;
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
            Destroyed?.Invoke(this, EventArgs.Empty);
            Despawn();
        }

        public virtual void TakeDamage(int damage)
        {
            currentHP -= damage;

            if (currentHP <= 0)
            {
                Destroy();
            }
            else
            {
                float damagePercent = 1f - (float)currentHP / healthPoints;
                int spriteIndex = Mathf.Clamp(Mathf.RoundToInt(sprites.Length * damagePercent), 0, sprites.Length - 1);
                spriteRenderer.sprite = sprites[spriteIndex];
            }
        }

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            initialSprite = spriteRenderer.sprite;
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameManager.LevelStarted += OnLevelStartedEventHandler;
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
        }

        private void OnLevelStartedEventHandler(object sender, int level)
        {
            if (sender is GameManager)
            {
                Spawn(transform.position, Vector2.up);
            }
        }
    }
}