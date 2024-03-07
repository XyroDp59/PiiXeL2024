using Script.GridSystem.Drawer;
using UnityEngine;

namespace Script.GridSystem
{
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] private GridType gridType;
        [SerializeField] private int gridSize;
        [SerializeField] public float cellSize;
        [SerializeField] private Color lineColor = Color.white;
        [SerializeField] private float lineWidth = 0.1f;

        private IGridDrawer _gridDrawer;

        private void Start()
        {
            _gridDrawer = GetGridDrawer();
            DrawGrid();
        }

        private void DrawGrid()
        {
            GameObject lineContainer = new GameObject("LineContainer");
            lineContainer.transform.SetParent(transform);
            _gridDrawer.DrawGrid(lineContainer, gridSize, cellSize, lineColor, lineWidth);
        }

        private IGridDrawer GetGridDrawer()
        {
            return gridType switch
            {
                GridType.Square => new SquareGridDrawer(),
                GridType.Triangle => new TriangleGridDrawer(),
                GridType.Hexagon => new HexagonGridDrawer(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(gridType), gridType, null)
            };
        }
    }
}