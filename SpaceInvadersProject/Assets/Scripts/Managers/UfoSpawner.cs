using UnityEngine;

namespace SpaceInvaders
{
    public class UfoSpawner : EntitySpawner<UfoController>
    {
        [SerializeField]
        private Transform alternativeSpawnPoint;

        private bool switchSpawnPoint;

        public override Vector2 GetPosition()
        {
            return switchSpawnPoint ? alternativeSpawnPoint.position : spawnPoint.position;
        }

        public override Vector2 GetDirection()
        {
            return switchSpawnPoint ? Vector2.right : Vector2.left;
        }

        public override void Respawn(float delay)
        {
            base.Respawn(delay);
            switchSpawnPoint = Random.Range(0, 2) > 0;
        }

        private void Start()
        {
            Respawn();
        }
    }
}