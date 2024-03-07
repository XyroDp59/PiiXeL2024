using UnityEngine;

namespace Script.GridSystem.Drawer
{
    public static class LineDrawer
    {
        public static void DrawLine(GameObject lineContainer, Vector3 start, Vector3 end, Color color, float width)
        {
            GameObject line = new GameObject("Line");
            line.transform.SetParent(lineContainer.transform);
            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = width;
            lr.endWidth = width;
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }
    }
}