using System;
using UnityEngine;

namespace SpaceInvaders
{
    public interface ISpawnable
    {
        event EventHandler Spawned;
        event EventHandler Despawned;

        void Spawn(Vector2 position, Vector2 direction);

        void Despawn();
    }
}