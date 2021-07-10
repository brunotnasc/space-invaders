using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public abstract class PlayerResourceManager : MonoBehaviour
    {
        [SerializeField, Range(1, 5)]
        protected int max = 3;
        [SerializeField]
        protected int initial = 3;
        [SerializeField]
        protected TMPro.TMP_Text counterText;
        [SerializeField]
        protected Transform iconPanel;
        [SerializeField]
        protected GameObject iconPrefab;

        protected int resourceCount;
        protected readonly List<GameObject> icons = new List<GameObject>();

        public virtual bool Consume()
        {
            bool hasResource = resourceCount > 0;
            resourceCount = Mathf.Max(--resourceCount, 0);
            UpdateDisplay();
            return hasResource;
        }

        public virtual void AddResourceUnit()
        {
            resourceCount = Mathf.Min(++resourceCount, max);
            UpdateDisplay();
        }

        public virtual void SetResourceCount(int count)
        {
            count = Mathf.Clamp(count, 0, max);
            resourceCount = count;
            UpdateDisplay();
        }

        protected void ResetValues()
        {
            resourceCount = Mathf.Clamp(initial, 0, max);
            UpdateDisplay();
        }

        protected void UpdateDisplay()
        {
            UpdateTextCounter("D1");
            UpdateIcons();
        }

        protected virtual void UpdateTextCounter(string format)
        {
            if (counterText != null)
            {
                counterText.text = GameManager.TMPMonospaceTags.Replace("*", resourceCount.ToString(format));
            }
        }

        protected virtual void UpdateIcons()
        {
            for (int i = 0; i < icons.Count; i++)
            {
                bool hasResource = (i + 1) <= resourceCount;
                icons[i].SetActive(hasResource);
            }
        }

        private void Awake()
        {
            Populate();
            ResetValues();
        }

        private void Populate()
        {
            // Remove placeholders
            if (iconPanel.childCount > 0)
            {
                foreach (Transform child in iconPanel)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < max; i++)
            {
                icons.Add(Instantiate(iconPrefab, iconPanel));
            }
        }
    }
}