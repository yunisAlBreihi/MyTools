using UnityEngine;

[CreateAssetMenu(fileName = "ReplayData", menuName = "ScriptableObjects/ReplayData", order = 1)]
public class ReplayDataScriptable : ScriptableObject
{
    public DataHolder data = null;
    public bool runReplay = false;

    public void StartReplay(DataHolder data) 
    {
        this.data = data;
        runReplay = true;
    }

    public void Clear()
    {
        runReplay = false;
        data = null;
    }
}
