using UnityEngine;

namespace SpaceInvaders
{
    public class PlayerSpawner : EntitySpawner<PlayerController>
    {
        public override Vector2 GetDirection()
        {
            return Vector2.up;
        }
    }
}