using System;
using System.IO;
using UnityEngine;

namespace SpaceInvaders
{
    public class ScoreboardManager : MonoBehaviour
    {
        private const int CounterMaxScore = 9999;
        private readonly string HighScorePath = $"{Application.streamingAssetsPath}{Path.DirectorySeparatorChar}HighestScore.txt";

        [SerializeField]
        private TMPro.TMP_Text p1ScoreCounter;
        [SerializeField]
        private TMPro.TMP_Text p2ScoreCounter;
        [SerializeField]
        private TMPro.TMP_Text highScoreCounter;

        private int highScore;
        private int currentScore;

        public void Register(InvaderController invader)
        {
            invader.Destroyed += OnInvaderDestroyedEventHandler;
        }

        private void Awake()
        {
            LoadHighScore();
            p2ScoreCounter.gameObject.SetActive(false);
        }

        private void OnApplicationQuit()
        {
            SaveHighScore();
        }

        private void LoadHighScore()
        {
            if (File.Exists(HighScorePath))
            {
                string text = File.ReadAllText(HighScorePath);
                if (int.TryParse(text, out int score))
                {
                    highScore = score;
                    UpdateScores();
                    return;
                }
            }
            Debug.LogWarning($"Could not load highest score from {HighScorePath}.");
        }

        private void SaveHighScore()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(HighScorePath));
            File.WriteAllText(HighScorePath, highScore.ToString());
            Debug.Log($"High score of {highScore} saved to {HighScorePath}.");
        }

        private void OnInvaderDestroyedEventHandler(object sender, EventArgs e)
        {
            if (sender is InvaderController invader)
            {
                currentScore += invader.Score;
                highScore = Mathf.Min(Mathf.Max(highScore, currentScore), CounterMaxScore);
                currentScore %= CounterMaxScore + 1;
                UpdateScores();
            }
        }

        private void UpdateScores()
        {
            p1ScoreCounter.text = GameManager.TMPMonospaceTags.Replace("*", currentScore.ToString("D4"));
            highScoreCounter.text = GameManager.TMPMonospaceTags.Replace("*", highScore.ToString("D4"));
        }
    }
}