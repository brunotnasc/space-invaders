using System;

namespace SpaceInvaders
{
    public interface IDestroyable
    {
        event EventHandler Destroyed;

        void TakeDamage(int damage);

        void Destroy();
    }
}