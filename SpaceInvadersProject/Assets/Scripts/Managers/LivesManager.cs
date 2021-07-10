using UnityEngine;

namespace SpaceInvaders
{
    [DisallowMultipleComponent]
    public class LivesManager : PlayerResourceManager
    {
        public int Lives => resourceCount;

        public bool GetExtraLife()
        {
            _ = Consume();
            return resourceCount > 0;
        }

        protected override void UpdateIcons()
        {
            for (int i = 0; i < icons.Count; i++)
            {
                bool hasResource = (i + 1) <= (resourceCount - 1);
                icons[i].SetActive(hasResource);
            }
        }
    }
}