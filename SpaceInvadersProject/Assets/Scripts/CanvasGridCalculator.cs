using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    /// <summary>
    /// Esta classe é responsável por fazer conversões entre coordendas de mundo e de canvas,
    /// e calcular a posição de células no grid.
    /// </summary>
    /// <remarks>
    /// As Canvas e CanvasScalers estão configurados para emular a resolução original do arcade
    /// do Space Invaders. Por isso, a resolução das Canvas é 224x256 e imagina-se um grid virtual
    /// de 28x32 células, cada célula com 8x8 pixels. Esse grid é usado como referência para 
    /// posicionar os elementos na tela.
    /// </remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasGridCalculator : MonoBehaviour
    {
        public const int CanvasGridCellSizeX = 8;
        public const int CanvasGridCellSizeY = 8;

        public int CanvasGridRows => (int)canvasScaler.referenceResolution.y / CanvasGridCellSizeY;
        public int CanvasGridColumns => (int)canvasScaler.referenceResolution.x / CanvasGridCellSizeX;

        private Vector2 ScreenSize => new Vector2(Screen.width, Screen.height);
        private Vector2 ScreenScale => ScreenSize / canvasResolution;

        private Camera mainCamera;
        private CanvasScaler canvasScaler;
        private Vector2 canvasResolution;

        public Vector2 WorldToCanvasPosition(Vector3 worldPosition)
        {
            Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
            screenPosition.y -= Screen.height;
            Vector2 canvasPosition = screenPosition / ScreenScale;
            return canvasPosition;
        }

        public Vector3 CanvasToWorldPosition(Vector2 canvasPosition)
        {
            Vector2 screenPosition = canvasPosition * ScreenScale;
            screenPosition.y += Screen.height;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0f;
            return worldPosition;
        }

        public Vector2 GetGridCellCenterCanvasPosition(int row, int column)
        {
            float canvasPositionX = CanvasGridCellSizeX * (column + 0.5f);
            float canvasPositionY = -CanvasGridCellSizeY * (row + 0.5f);
            Vector2 canvasPosition = new Vector2(
                Mathf.RoundToInt(canvasPositionX),
                Mathf.RoundToInt(canvasPositionY));
            return canvasPosition;
        }

        private void Awake()
        {
            mainCamera = Camera.main;
            canvasScaler = GetComponent<CanvasScaler>();
            canvasResolution = canvasScaler.referenceResolution;
        }
    }
}