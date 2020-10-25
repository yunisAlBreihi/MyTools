using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] private PlayerSave player;
    
    private PlayerData playerData = new PlayerData();
    private IUnit unit;
    private string jsonPath;
    private string jsonFileName;
    private string jsonFile;

    private void Awake()
    {
        jsonPath = Application.dataPath + "/DebugStats/";
        jsonFileName = player.name + "_" + DateTime.Now.Second + "_debugStats.json";
        unit = player.GetComponent<IUnit>();
        playerData.positions = new List<Vector3>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    private void Save()
    {
        SaveJsonFile();
        Debug.Log("Saved pos: " + playerData.positions[playerData.positions.Count - 1]);
    }

    private void Load()
    {
        LoadJsonFile();
        StartCoroutine(MovePlayer());
    }

    IEnumerator MovePlayer() 
    {
        WaitForSeconds timeToWait = new WaitForSeconds(1.0f);
        foreach (var pos in playerData.positions)
        {
            player.SetPosition(pos);
            Debug.Log("Loaded pos: " + pos);
            yield return timeToWait;
        }
    }

    private void LoadJsonFile()
    {
        //Load
        jsonFile = File.ReadAllText(jsonPath + jsonFileName);
        playerData = JsonUtility.FromJson<PlayerData>(jsonFile);
    }

    private void SaveJsonFile() 
    {
        //Save
        playerData.positions.Add(player.GetPosition());
        jsonFile = JsonUtility.ToJson(playerData);
        File.WriteAllText(jsonPath + jsonFileName, jsonFile);
    }
}
