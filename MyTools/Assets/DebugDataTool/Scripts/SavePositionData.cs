using System.Collections.Generic;
using UnityEngine;

public class SavePositionData : SaveDataBaseClass
{
    //Camera cam;

    protected override void Start()
    {
        base.Start();

        //cam = gameObject.GetComponentInChildren<Camera>();
    }

    public override void OnAddData()
    {
        data.positions.Add(transform.position);
    }
}