using System;
using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class InvaderController : EntityController, IShooter, ICollisionReporter
    {
        public event EventHandler<Vector2> PrimaryFired;
        public event EventHandler<Vector2> SecondaryFired;
        public event EventHandler<GameObject> Collided;

        public int Score => score;

        [SerializeField]
        protected int score;
        [SerializeField, Range(0f, 1f)]
        protected float chanceToFire;
        [SerializeField, Space]
        protected Sprite poseA;
        [SerializeField]
        protected Sprite poseB;

        private bool isLead;
        private Coroutine tryToFireCoroutine;

        public void MoveTo(Vector2 position)
        {
            rb2D.MovePosition(position);
            ChangePose();
        }

        public void CheckIfIsLead()
        {
            if (isLead)
            {
                return;
            }
            int layerMask = 1 << LayerMask.NameToLayer("Enemy");
            Vector2 offset = Vector2.down * 40f;
            RaycastHit2D hit = Physics2D.Raycast(RBPosition + offset, Vector2.down, Screen.height, layerMask);
            isLead = hit.collider == GetComponent<Collider>();
        }

        public override void Spawn(Vector2 position, Vector2 direction)
        {
            base.Spawn(position, direction);
            StartFiring();
            isLead = false;
        }

        public override void Destroy()
        {
            base.Destroy();
            StopFiring();
        }

        protected override void Awake()
        {
            base.Awake();
            gameManager.Register(this as InvaderController);
            gameManager.Register(this as IShooter);
            gameManager.LevelStarted += OnLevelStartedEventHandler;
            gameManager.LevelFinished += OnLevelFinishedEventHandler;
            gameManager.GameOver += OnGameOverEventHandler;
        }

        protected override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            Collided?.Invoke(this, other);
        }

        private void ChangePose()
        {
            bool isPoseA = spriteRenderer.sprite == poseA;
            spriteRenderer.sprite = isPoseA ? poseB : poseA;
        }

        private void FirePrimaryWeapon()
        {
            PrimaryFired?.Invoke(this, transform.position);
        }

        private IEnumerator TryToFireCoroutine()
        {
            while (true)
            {
                bool canFire = isLead && !IsExploding && isActiveAndEnabled && gameManager.Player.isActiveAndEnabled;
                if (canFire)
                {
                    float threshold = CalculateFireThreshold();
                    if (chanceToFire > threshold)
                    {
                        FirePrimaryWeapon();
                    }
                }

                float interval = UnityEngine.Random.Range(0.25f, 1f);
                yield return new WaitForSeconds(interval);
            }
        }

        private float CalculateFireThreshold()
        {
            float playerPositionX = gameManager.Player.RBPosition.x;
            float horizontalDistance = Mathf.Abs(playerPositionX - RBPosition.x);
            float aligmentWithPlayer = Mathf.Clamp01(1f - horizontalDistance / Screen.width);
            float thresholdModifier = Mathf.SmoothStep(1f, 0.1f, aligmentWithPlayer);
            float threshold = UnityEngine.Random.Range(0f, 1f);
            return threshold * thresholdModifier;
        }

        private void OnLevelStartedEventHandler(object sender, int level)
        {
            if (sender is GameManager)
            {
                StartFiring();
            }
        }

        private void OnLevelFinishedEventHandler(object sender, EventArgs e)
        {
            if (sender is GameManager)
            {
                StopFiring();
            }
        }

        private void OnGameOverEventHandler(object sender, EventArgs e)
        {
            if (sender is GameManager)
            {
                StopFiring();
            }
        }

        private void StartFiring()
        {
            if (tryToFireCoroutine == null && isActiveAndEnabled)
            {
                tryToFireCoroutine = StartCoroutine(TryToFireCoroutine());
            }
        }

        private void StopFiring()
        {
            if (tryToFireCoroutine != null)
            {
                StopCoroutine(tryToFireCoroutine);
                tryToFireCoroutine = null;
            }
        }

#if UNITY_EDITOR
        public bool debugIsLead;

        private void Update()
        {
            if (debugIsLead)
            {
                spriteRenderer.color = isLead ? Color.cyan : Color.yellow;
            }
        }
#endif
    }
}