using UnityEngine;

namespace Script.GridSystem.Drawer
{
    public interface IGridDrawer
    {
        void DrawGrid(GameObject lineContainer, int gridSize, float cellSize, Color lineColor, float lineWidth);
    }
}