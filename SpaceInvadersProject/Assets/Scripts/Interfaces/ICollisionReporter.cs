using System;
using UnityEngine;

namespace SpaceInvaders
{
    public interface ICollisionReporter
    {
        event EventHandler<GameObject> Collided;
    }
}