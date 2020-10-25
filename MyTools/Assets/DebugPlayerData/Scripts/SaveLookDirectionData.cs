using System.Collections.Generic;
using UnityEngine;

public class SaveLookDirectionData : SaveDataBaseClass
{

    protected override void Start()
    {
        base.Start();
    }

    public override void OnAddData()
    {
        data.lookDirections.Add(transform.forward);
    }
}