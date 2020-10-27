using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataHolder
{
    public float capturesPerSecond = 0.0f;
    public List<Vector3> positions = null;
    public List<Vector3> lookDirections = null;
}
