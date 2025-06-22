using System.Collections.Generic;
using UnityEngine;

namespace Code.Path
{
    public class PathGenerator : MonoBehaviour
    {
        [Header("Path Settings")]
        [SerializeField] private Transform startingPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform[] pathPoints;
        [SerializeField] private LineRenderer pathRenderer;
        [SerializeField] private int curveResolution = 50;
        [SerializeField] private Color gizmoColor = Color.green;

        private readonly List<Vector3> smoothPath = new();

        private void Start()
        {
            if (pathPoints.Length >= 1)
            {
                GenerateSmoothPath();
                DrawCurvedPath();
            }
        }

        private void GenerateSmoothPath()
        {
            smoothPath.Clear();

            // Build full list of points including start and end
            List<Vector3> allPoints = new();

            if (startingPoint != null)
                allPoints.Add(startingPoint.position);

            foreach (var p in pathPoints)
                if (p != null)
                    allPoints.Add(p.position);

            if (endPoint != null)
                allPoints.Add(endPoint.position);

            if (allPoints.Count < 4)
            {
                // Fallback: draw simple line
                smoothPath.AddRange(allPoints);
                return;
            }

            // Smooth with Catmull-Rom
            for (int i = 0; i < allPoints.Count - 3; i++)
            {
                Vector3 p0 = allPoints[i];
                Vector3 p1 = allPoints[i + 1];
                Vector3 p2 = allPoints[i + 2];
                Vector3 p3 = allPoints[i + 3];

                for (int j = 0; j < curveResolution; j++)
                {
                    float t = j / (float)curveResolution;
                    Vector3 point = GetCatmullRomPosition(t, p0, p1, p2, p3);
                    smoothPath.Add(point);
                }
            }

            // Add final endpoint to guarantee it connects
            smoothPath.Add(allPoints[^1]);
        }

        private void DrawCurvedPath()
        {
            if (pathRenderer == null || smoothPath.Count < 2) return;

            pathRenderer.positionCount = smoothPath.Count;
            pathRenderer.SetPositions(smoothPath.ToArray());
        }

        private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            return 0.5f * ((2 * p1) +
                          (-p0 + p2) * t +
                          (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
                          (-p0 + 3 * p1 - 3 * p2 + p3) * t3);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false)
                GenerateSmoothPath(); // Live update in editor

            Gizmos.color = gizmoColor;
            for (int i = 0; i < smoothPath.Count - 1; i++)
            {
                Gizmos.DrawLine(smoothPath[i], smoothPath[i + 1]);
            }

            Gizmos.color = Color.red;
            if (startingPoint != null) Gizmos.DrawSphere(startingPoint.position, 0.2f);
            if (endPoint != null) Gizmos.DrawSphere(endPoint.position, 0.2f);

            Gizmos.color = Color.blue;
            foreach (var point in pathPoints)
            {
                if (point != null)
                    Gizmos.DrawSphere(point.position, 0.15f);
            }
        }
    }
}
