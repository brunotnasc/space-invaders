using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    public class WaveSpawner : MonoBehaviour
    {
        public event EventHandler WaveSpawned;
        public event EventHandler WaveDestroyed;

        public const int WaveRows = 5;
        public const int WaveColumns = 11;

        [SerializeField]
        private Vector2Int gridOriginCellOffset = new Vector2Int(8, 3);

        [Header("Enemy Prefabs")]
        [SerializeField]
        private GameObject smallInvaderPrefab;
        [SerializeField]
        private GameObject mediumInvaderPrefab;
        [SerializeField]
        private GameObject largeInvaderPrefab;

        private GameManager gameManager;
        private int spawnRowAdvance;
        private int invaderCount;

        private readonly List<InvaderController> invaderPool = new List<InvaderController>();

        private void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameManager.LevelStarted += OnLevelStartedEventHandler;
            FillInvaderPool();
        }

        public void SpawnWave(int rowAdvance)
        {
            spawnRowAdvance = rowAdvance;
            _ = StartCoroutine(SpawnWaveCoroutine());
        }

        private IEnumerator SpawnWaveCoroutine()
        {
            yield return null;

            int index = 0;
            for (int row = 0; row < WaveRows; row++)
            {
                int bottomUpRow = (WaveRows - (row + 1)) * 2 + spawnRowAdvance;

                for (int column = 0; column < WaveColumns; column++)
                {
                    int gridRow = bottomUpRow + gridOriginCellOffset.x;
                    int gridColumn = column * 2 + gridOriginCellOffset.y;
                    SpawnInvader(index, gridRow, gridColumn);
                    index++;
                    yield return new WaitForSeconds(0.015f);
                }
            }

            WaveSpawned?.Invoke(this, EventArgs.Empty);
        }

        private void FillInvaderPool()
        {
            for (int row = 0; row < WaveRows; row++)
            {
                GameObject invaderPrefab = GetInvaderPrefab(row);

                for (int column = 0; column < WaveColumns; column++)
                {
                    invaderPool.Add(InstantiateInvader(invaderPrefab));
                }
            }
        }

        private GameObject GetInvaderPrefab(int row)
        {
            return row switch
            {
                0 => largeInvaderPrefab,
                1 => largeInvaderPrefab,
                2 => mediumInvaderPrefab,
                3 => mediumInvaderPrefab,
                _ => smallInvaderPrefab
            };
        }

        private InvaderController InstantiateInvader(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, transform);
            InvaderController invader = go.GetComponent<InvaderController>();
            invader.Spawned += OnInvaderSpawnedEventHandler;
            invader.Despawned += OnInvaderDespawnedEventHandler;
            go.SetActive(false);
            return invader;
        }

        private void OnInvaderSpawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is InvaderController)
            {
                invaderCount++;
            }
        }

        private void OnInvaderDespawnedEventHandler(object sender, EventArgs e)
        {
            if (sender is InvaderController)
            {
                invaderCount--;

                if (invaderCount <= 0)
                {
                    WaveDestroyed?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void OnLevelStartedEventHandler(object sender, int level)
        {
            SpawnWave(level);
        }

        private void SpawnInvader(int index, int row, int column)
        {
            InvaderController invader = invaderPool[index];
            Vector2 canvasPosition = gameManager.GetGridCellCenterCanvasPosition(row, column);
            canvasPosition.x += CanvasGridCalculator.CanvasGridCellSizeX * 0.5f;
            Vector3 worldPosition = gameManager.CanvasToWorldPosition(canvasPosition);
            invader.Spawn(worldPosition, Vector2.down);
        }
    }
}