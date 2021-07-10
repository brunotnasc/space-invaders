using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class UfoController : InvaderController
    {
        [SerializeField]
        private float speed;

        private FlyingScore flyingScore;

        public override void Spawn(Vector2 position, Vector2 direction)
        {
            base.Spawn(position, direction);
            score = UnityEngine.Random.Range(1, 7) * 50;
            Vector2 velocity = direction * speed;
            rb2D.velocity = velocity;
        }

        protected override void Awake()
        {
            base.Awake();
            flyingScore = GameObject.FindGameObjectWithTag("FlyingScore").GetComponent<FlyingScore>();
            Destroyed += OnDestroyedEventHandler;
        }

        private void OnDestroyedEventHandler(object sender, EventArgs e)
        {
            flyingScore.DisplayScore(score, transform.position);
        }
    }
}