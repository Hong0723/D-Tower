using UnityEngine;

public class PathGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Transform[] points = GetComponentsInChildren<Transform>();
        Gizmos.color = Color.yellow;

        for (int i = 1; i < points.Length - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
    }
}
