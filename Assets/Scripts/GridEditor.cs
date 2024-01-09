using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    public Material lineMaterial;
    private float gridSize = 1.0f; 

    void Start()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        for (int i = 0; i <= 4; i++)
        {
            
            CreateLine(new Vector3(i * gridSize, 0, 0), new Vector3(i * gridSize, 0, 4 * gridSize));

            
            CreateLine(new Vector3(0, 0, i * gridSize), new Vector3(4 * gridSize, 0, i * gridSize));
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.parent = this.transform;

        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { start, end });
        lineRenderer.startWidth = 0.05f; 
        lineRenderer.endWidth = 0.05f;
    }
}
