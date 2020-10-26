using System.Collections.Generic;
using UnityEngine;

public class SaveLookDirectionData : SaveDataBaseClass
{
    Camera cam = null;

    protected override void Start()
    {
        base.Start();
        cam = GetComponentInChildren<Camera>();
    }

    public override void OnAddData()
    {
        data.lookDirections.Add(transform.forward);
    }
}