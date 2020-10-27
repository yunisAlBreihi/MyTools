using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DebugLine : MonoBehaviour
{
    private DataHolder data = null;
    private LineRenderer lineRenderer = null;

    public void Create(DataHolder dataHolder)
    {
        data = dataHolder;
        GenerateLine();
    }

    private void GenerateLine()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = data.positions.Count;
        lineRenderer.SetPositions(data.positions.ToArray());
        lineRenderer.Simplify(0.5f);
    }
}
