using Script.GridSystem.Drawer;
using UnityEngine;

namespace Script.GridSystem
{
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] public GridType gridType ;
        [SerializeField] private int gridSize;
        [SerializeField] public float cellSize;
        public float distBetweenCells;
        [SerializeField] private Color lineColor = Color.white;
        [SerializeField] private float lineWidth = 0.1f;

        private IGridDrawer _gridDrawer;

        private void Start()
        {
            _gridDrawer = GetGridDrawer();
            DrawGrid();

            switch (gridType)
            {
                case GridType.Hexagon: distBetweenCells = cellSize * Mathf.Sqrt(3); break;
                case GridType.Square: distBetweenCells = cellSize; break;
                case GridType.Triangle: distBetweenCells = cellSize / Mathf.Sqrt(3); break;
                default: distBetweenCells = cellSize; break;
            }
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