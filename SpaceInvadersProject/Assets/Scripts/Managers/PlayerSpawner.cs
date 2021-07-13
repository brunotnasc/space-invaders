using UnityEngine;

namespace SpaceInvaders
{
    public class PlayerSpawner : EntitySpawner<PlayerController>
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Assert(spawnPoints.Length == 1, $"{nameof(UfoSpawner)} should have 1 spawn point.");
        }

        protected override Vector2 GetSpawnPosition()
        {
            return spawnPoints[0].position;
        }

        protected override Vector2 GetSpawnDirection()
        {
            return Vector2.up;
        }
    }
}