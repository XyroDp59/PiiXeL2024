using UnityEngine;
using static Script.GridSystem.Drawer.LineDrawer;

namespace Script.GridSystem.Drawer
{
    public class TriangleGridDrawer : IGridDrawer
    {
        public void DrawGrid(GameObject lineContainer, int gridSize, float cellSize, Color lineColor, float lineWidth)
        {
            float sqrt3 = Mathf.Sqrt(3f);
            for (int i = 0; i <= gridSize / 2 + 1; i++)
            {
                for (int j = 0; j <= gridSize / 2 + 1; j++)
                {
                    Vector3 center = new Vector3(i * 1.5f * cellSize, j * sqrt3 * cellSize, 0f);
                    if (i % 2 == 1)
                    {
                        center.y += sqrt3 * cellSize * 0.5f;
                    }
                    DrawTriangleHexagon(lineContainer, center, cellSize, lineColor, lineWidth);
                }
            }
        }

        private static void DrawTriangleHexagon(GameObject lineContainer, Vector3 center, float cellSize, Color lineColor, float lineWidth)
        {
            for (int i = 0; i < 6; i++)
            {
                float angle = 60f * i * Mathf.Deg2Rad;
                Vector3 startPos = center + new Vector3(cellSize * Mathf.Cos(angle), cellSize * Mathf.Sin(angle), 0f);
                Vector3 endPos = center + new Vector3(cellSize * Mathf.Cos(angle + 60f * Mathf.Deg2Rad), cellSize * Mathf.Sin(angle + 60f * Mathf.Deg2Rad), 0f);
                DrawLine(lineContainer, startPos, endPos, lineColor, lineWidth);
                DrawTriangle(lineContainer, center, startPos, endPos, lineColor, lineWidth);
            }
        }

        private static void DrawTriangle(GameObject lineContainer, Vector3 center, Vector3 startPos, Vector3 endPos, Color color, float width)
        {
            DrawLine(lineContainer, startPos, center, color, width);
            DrawLine(lineContainer, center, endPos, color, width);
        }
    }
}