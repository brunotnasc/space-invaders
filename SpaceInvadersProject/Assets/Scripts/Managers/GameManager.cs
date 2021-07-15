using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        private const int MonospaceSize = 8;
        public static string TMPMonospaceTags = $"<mspace=mspace={MonospaceSize}>*</mspace>";

        public event EventHandler<int> LevelStarted;
        public event EventHandler LevelFinished;
        public event EventHandler GameOver;

        public bool IsGameOver { get; private set; }
        public PlayerController Player { get; private set; }
        public UfoController Ufo { get; private set; }
        public IReadOnlyList<InvaderController> Invaders => invaders.AsReadOnly();

        private readonly List<InvaderController> invaders = new List<InvaderController>();

        [SerializeField, Header("Managers")]
        private ScoreboardManager scoreboardManager;
        [SerializeField]
        private LivesManager livesManager;
        [SerializeField]
        private WaveController waveController;
        [SerializeField, Header("Spawners")]
        private PlayerSpawner playerSpawner;
        [SerializeField]
        private UfoSpawner ufoSpawner;
        [SerializeField]
        private WaveSpawner waveSpawner;
        [SerializeField]
        private ProjectileSpawner projectileSpawner;
        [SerializeField, Space]
        private GameObject gameOverText;
        [SerializeField, Space]
        private CanvasGridCalculator grid;

        private int currentLevel = 0;

        public void Register(IShooter shooter)
        {
            projectileSpawner.Register(shooter);
        }

        public void Register(InvaderController invader)
        {
            scoreboardManager.Register(invader);

            if (invader is UfoController ufo)
            {
                ufo.gameObject.SetActive(false);
                Ufo = ufo;
            }
            else
            {
                waveController.Register(invader);
                invaders.Add(invader);
                invader.Collided += OnInvaderCollidedEventHandler;
            }
        }

        public void Register(PlayerController player)
        {
            player.Spawned += OnPlayerSpawnedEventHandler;
            player.Destroyed += OnPlayerDestroyedEventHandler;
            player.gameObject.SetActive(false);
            Player = player;
        }

        public Vector2 WorldToCanvasPosition(Vector3 worldPosition)
        {
            return grid.WorldToCanvasPosition(worldPosition);
        }

        public Vector3 CanvasToWorldPosition(Vector2 canvasPosition)
        {
            return grid.CanvasToWorldPosition(canvasPosition);
        }

        public Vector2 GetGridCellCenterCanvasPosition(int row, int column)
        {
            return grid.GetGridCellCenterCanvasPosition(row, column);
        }

        private void Awake()
        {
            gameOverText.SetActive(false);
            waveSpawner.WaveSpawned += OnWaveSpawnedEventHandler;
            waveSpawner.WaveDestroyed += OnWaveDestroyedEventHandler;

            Cursor.visible = false;
        }

        private void Start()
        {
            StartLevel(0);
        }

        private void StartLevel(int level)
        {
            currentLevel = level;
            LevelStarted?.Invoke(this, level);
        }

        private void FinishLevel()
        {
            LevelFinished?.Invoke(this, EventArgs.Empty);
            Player.Despawn();
            waveController.Stop();
            StartLevel((currentLevel + 1) % 8);
        }

        private void OnGameOver()
        {
            gameOverText.SetActive(true);
            waveController.Stop();
            IsGameOver = true;
            GameOver?.Invoke(this, EventArgs.Empty);
        }

        private void OnPlayerSpawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is PlayerController)
            {
                waveController.Play();
            }
        }

        private void OnPlayerDestroyedEventHandler(object sender, EventArgs e)
        {
            if (sender is PlayerController)
            {
                waveController.Pause();

                if (livesManager.GetExtraLife())
                {
                    playerSpawner.Spawn();
                }
                else
                {
                    OnGameOver();
                }
            }
        }

        private void OnWaveSpawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is WaveSpawner)
            {
                waveController.Play();
                playerSpawner.Spawn(1f);
            }
        }

        private void OnWaveDestroyedEventHandler(object sender, EventArgs e)
        {
            if (sender is WaveSpawner)
            {
                FinishLevel();
            }
        }

        private void OnInvaderCollidedEventHandler(object sender, GameObject other)
        {
            if (sender is InvaderController)
            {
                if (other.CompareTag("PlayerBase"))
                {
                    while (livesManager.GetExtraLife()) ;
                    Player.Destroy();
                    OnGameOver();
                }
            }
        }
    }
}