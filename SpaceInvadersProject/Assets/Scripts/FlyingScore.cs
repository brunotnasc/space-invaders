using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform), typeof(TMPro.TMP_Text))]
    public class FlyingScore : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 2f)]
        private float duration = 0.5f;

        private GameManager gameManager;
        private RectTransform rectTransform;
        private TMPro.TMP_Text text;

        public void DisplayScore(int score, Vector2 worldPosition)
        {
            rectTransform.anchoredPosition = gameManager.WorldToCanvasPosition(worldPosition);
            text.text = GameManager.TMPMonospaceTags.Replace("*", score.ToString("D3"));

            gameObject.SetActive(true);
            _ = StartCoroutine(HideScoreCoroutine());
        }

        private void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            rectTransform = GetComponent<RectTransform>();
            text = GetComponent<TMPro.TMP_Text>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator HideScoreCoroutine()
        {
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }
    }
}