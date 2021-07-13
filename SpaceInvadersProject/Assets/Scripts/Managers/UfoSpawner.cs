using UnityEngine;

namespace SpaceInvaders
{
    public class UfoSpawner : EntitySpawner<UfoController>
    {
        private bool useAlternativeSpawnPoint;

        public override void Spawn(float delay)
        {
            base.Spawn(delay);
            useAlternativeSpawnPoint = Random.Range(0, 2) > 0;
        }

        protected override void Awake()
        {
            base.Awake();
            Debug.Assert(spawnPoints.Length == 2, $"{nameof(UfoSpawner)} should have 2 spawn points.");
        }

        protected override Vector2 GetSpawnPosition()
        {
            return useAlternativeSpawnPoint ? spawnPoints[1].position : spawnPoints[0].position;
        }

        protected override Vector2 GetSpawnDirection()
        {
            return useAlternativeSpawnPoint ? Vector2.right : Vector2.left;
        }

        private void Start()
        {
            Spawn();
        }
    }
}