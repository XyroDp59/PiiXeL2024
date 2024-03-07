using UnityEngine;
using static Script.GridSystem.Drawer.LineDrawer;

namespace Script.GridSystem.Drawer
{
    public class SquareGridDrawer : IGridDrawer
    {
        public void DrawGrid(GameObject lineContainer, int gridSize, float cellSize, Color lineColor, float lineWidth)
        {
            for (int i = 0; i <= gridSize; i++)
            {
                DrawLine(lineContainer, new Vector3(0f, i * cellSize, 0f), new Vector3(gridSize * cellSize, i * cellSize, 0f), lineColor, lineWidth);
                DrawLine(lineContainer, new Vector3(i * cellSize, 0f, 0f), new Vector3(i * cellSize, gridSize * cellSize, 0f), lineColor, lineWidth);
            }
        }
    }
}