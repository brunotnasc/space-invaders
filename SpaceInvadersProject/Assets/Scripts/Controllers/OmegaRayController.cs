using System;
using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScreenDistorter))]
    public class OmegaRayController : MonoBehaviour, ISpawnable
    {
        private const int Damage = 1000;
        private const float ScaleChangeTime = 0.5f;

        public event EventHandler Spawned;
        public event EventHandler Despawned;

        public bool IsFiring { get; private set; }

        [SerializeField, Range(1f, 5f)]
        private float duration = 2f;
        [SerializeField, Range(1f, 6f)]
        private float chargeTime = 3f;
        [SerializeField, Space]
        private LayerMask doDamageOnCollision;
        [SerializeField, Space]
        private GameObject ray;

        private GameManager gameManager;
        private ScreenDistorter screenDistorter;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        public void SetDurationAndChargeTime(float duration, float chargeTime)
        {
            this.duration = duration;
            this.chargeTime = chargeTime;
        }
#endif

        public void Spawn(Vector2 position, Vector2 direction)
        {
            transform.position = position;
            transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            ray.SetActive(false);
            Fire();
            Spawned?.Invoke(this, EventArgs.Empty);
        }

        public void Despawn()
        {
            gameObject.SetActive(false);

            if (IsFiring)
            {
                StopAllCoroutines();
                screenDistorter.Abort();
                IsFiring = false;
            }

            Despawned?.Invoke(this, EventArgs.Empty);
        }

        private void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            screenDistorter = GetComponent<ScreenDistorter>();
            gameManager.Player.Exploding += (s, e) => Despawn();
            gameManager.Player.Despawned += (s, e) => Despawn();
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnCollision(collision.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollision(collision.gameObject);
        }

        private void Fire()
        {
            if (isActiveAndEnabled)
            {
                IsFiring = true;
                _ = StartCoroutine(FireCoroutine());
            }
        }

        private void OnCollision(GameObject other)
        {
            int layer = 1 << other.layer;
            IDestroyable destroyable = other.GetComponent<IDestroyable>();
            bool isDamageable = destroyable != null && (layer & doDamageOnCollision.value) != 0;
            if (isDamageable)
            {
                destroyable.TakeDamage(Damage);
            }
        }

        private IEnumerator FireCoroutine()
        {
            yield return ChargeCoroutine();

            screenDistorter.DistortScreen(ScaleChangeTime * 2f + duration);

            yield return LerpScaleXCoroutine(0f, 1f, ray.transform);

            yield return new WaitForSeconds(duration);

            yield return LerpScaleXCoroutine(1f, 0f, transform);

            Despawn();
        }

        private IEnumerator ChargeCoroutine()
        {
            yield return new WaitForSeconds(chargeTime);

            ray.SetActive(true);
        }

        private IEnumerator LerpScaleXCoroutine(float from, float to, Transform transform)
        {
            float time = 0f;
            while (time < ScaleChangeTime)
            {
                float t = time / ScaleChangeTime;
                time += Time.deltaTime;

                Vector3 scale = new Vector3(Mathf.Lerp(from, to, t), 1f, 1f);
                transform.localScale = scale;

                yield return null;
            }
            ray.transform.localScale = new Vector3(to, 1f, 1f);
        }
    }
}