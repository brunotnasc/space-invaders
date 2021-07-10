using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceInvaders
{
    public class WaveController : MonoBehaviour
    {
        const string RightEdgeTag = "RightEdge";
        const string LeftEdgeTag = "LeftEdge";

        public event EventHandler WaveAdvancing;

        private readonly List<InvaderController> invaders = new List<InvaderController>();

        [SerializeField]
        private int stepSizeX = CanvasGridCalculator.CanvasGridCellSizeX / 4;
        [SerializeField]
        private int stepSizeY = CanvasGridCalculator.CanvasGridCellSizeY;

        private GameManager gameManager;
        private bool isPlaying;
        private bool isRunning;
        private bool shouldAdvance;
        private Vector2 step;
        private string expectedEdge;
        private int headIndex;

        public void Register(InvaderController invader)
        {
            invaders.Add(invader);
            invader.Collided += OnInvaderCollidedEventHandler;
        }

        public void Play()
        {
            isPlaying = true;

            if (!isRunning)
            {
                isRunning = true;
                shouldAdvance = false;
                step = new Vector2(stepSizeX, 0f);
                expectedEdge = RightEdgeTag;
                headIndex = 0;
            }
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public void Stop()
        {
            isRunning = false;
            isPlaying = false;
        }

        private void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        private void FixedUpdate()
        {
            UpdateWave();
        }

        private void UpdateWave()
        {
            if (!isRunning || !isPlaying)
            {
                return;
            }

            int removed = invaders.Sum(i => i.isActiveAndEnabled ? 0 : 1);
            float timePerElement = 1f / (invaders.Count - removed);
            int invadersToUpdate = Mathf.Max(1, Mathf.RoundToInt(timePerElement / Time.fixedDeltaTime));

            // Wave loop
            for (int i = 0; i < invadersToUpdate; i++)
            {
                int index = (i + headIndex) % invaders.Count;

                // Beginning of the wave
                if (index == 0)
                {
                    if (shouldAdvance)
                    {
                        shouldAdvance = false;
                        step.x *= -1f;
                        step.y = -stepSizeY;
                        WaveAdvancing?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        step.y = 0;
                    }
                }

                InvaderController invader = invaders[index];

                if (invader.IsExploding || !invader.isActiveAndEnabled)
                {
                    // Skip destroyed invaders
                    continue;
                }

                invader.CheckIfIsLead();
                MoveInvader(invader);
            }

            headIndex += invadersToUpdate;
            headIndex %= invaders.Count;
        }

        private void MoveInvader(InvaderController invader)
        {
            Vector2 canvasPosition = gameManager.WorldToCanvasPosition(invader.RBPosition);
            Vector2 targetCanvasPosition = canvasPosition + step;
            Vector2 targetWorldPosition = gameManager.CanvasToWorldPosition(targetCanvasPosition);
            invader.MoveTo(targetWorldPosition);

        }

        private void OnInvaderCollidedEventHandler(object sender, GameObject other)
        {
            if (sender is InvaderController)
            {
                if (other.CompareTag(expectedEdge) && !shouldAdvance)
                {
                    shouldAdvance = true;
                    expectedEdge = expectedEdge.Equals(RightEdgeTag) ? LeftEdgeTag : RightEdgeTag;
                }
            }
        }
    }
}