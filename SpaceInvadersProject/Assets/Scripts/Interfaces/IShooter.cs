using System;
using UnityEngine;

namespace SpaceInvaders
{
    public interface IShooter
    {
        event EventHandler<Vector2> PrimaryFired;
        event EventHandler<Vector2> SecondaryFired;
    }
}