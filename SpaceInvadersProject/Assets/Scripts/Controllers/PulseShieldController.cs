using System;
using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class PulseShieldController : MonoBehaviour, ISpawnable
    {
        public event EventHandler Spawned;
        public event EventHandler Despawned;
        public event EventHandler Discharged;
        public event EventHandler Recharged;

        [SerializeField, Range(0.1f, 3f)]
        private float duration = 1f;
        [SerializeField, Range(0f, 60f)]
        private float rechargeTime = 30f;
        [SerializeField, Space]
        private RectTransform pulseShieldIndicator;

        private GameManager gameManager;
        private bool isCharged;
        private Coroutine autoDespawnCoroutine;
        private Coroutine rechargeCoroutine;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        public void SetDurationAndRechargeTime(float duration, float rechargeTime)
        {
            this.duration = duration;
            this.rechargeTime = rechargeTime;
        }
#endif

        public void Spawn(Vector2 position, Vector2 direction)
        {
            if (!isCharged)
            {
                return;
            }

            transform.position = position;
            gameObject.SetActive(true);
            pulseShieldIndicator.gameObject.SetActive(false);
            isCharged = false;
            Discharged?.Invoke(this, EventArgs.Empty);

            if (autoDespawnCoroutine == null && isActiveAndEnabled)
            {
                autoDespawnCoroutine = StartCoroutine(AutoDespawnCoroutine());
            }

            Spawned?.Invoke(this, EventArgs.Empty);
        }

        public void Despawn()
        {
            if (rechargeCoroutine == null && isActiveAndEnabled)
            {
                rechargeCoroutine = gameManager.StartCoroutine(RechargeCoroutine());
            }

            gameObject.SetActive(false);
            Despawned?.Invoke(this, EventArgs.Empty);
        }

        private void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameObject.SetActive(false);
            isCharged = true;
        }

        private IEnumerator AutoDespawnCoroutine()
        {
            yield return new WaitForSeconds(duration);
            Despawn();
            autoDespawnCoroutine = null;
        }

        private IEnumerator RechargeCoroutine()
        {
            yield return new WaitForSeconds(rechargeTime);
            pulseShieldIndicator.gameObject.SetActive(true);
            isCharged = true;
            Recharged?.Invoke(this, EventArgs.Empty);
            rechargeCoroutine = null;
        }
    }
}