using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] private List<SaveDataBaseClass> saveDatas = new List<SaveDataBaseClass>();
    [SerializeField] float capturesPerSecond = 5.0f;
    [SerializeField]private bool capturingData = true;
    [SerializeField] private ReplayPlayerData player;

    private string jsonFile;

    public string JsonPath { get; private set; }
    public string JsonPrefix { get; private set; }

    private DataHolder playerData;

    private void Awake()
    {
        JsonPath = Directory.CreateDirectory(Application.dataPath + "/DebugStats/" + SceneManager.GetActiveScene().name + "/").FullName;
        JsonPrefix = "_" + SceneManager.GetActiveScene().name + "_" + Random.Range(0,10000000) + "_debugStats.json";

        if (capturingData == true)
        {
            StartCoroutine(AddDataTimer());
        }
        else
        {
            Load();
        }
    }

    private IEnumerator AddDataTimer() 
    {
        WaitForSeconds wait = new WaitForSeconds(1.0f / capturesPerSecond);
        while (capturingData == true)
        {
            OnAddData();
            yield return wait;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnSave();
        }
    }

    private void OnAddData()
    {
        foreach (var save in saveDatas)
        {
            save.OnAddData();
        }
    }

    private void OnSave()
    {
        foreach (var save in saveDatas)
        {
            save.OnSave();
        }

        Debug.Log("Saved Json!");
    }

    public void AddSaveData(SaveDataBaseClass saveData)
    {
        saveDatas.Add(saveData);
    }

    private void Save()
    {
        foreach (var save in saveDatas)
        {
            jsonFile = JsonUtility.ToJson(save);
            File.WriteAllText(JsonPath + JsonPrefix, jsonFile);
        }
    }

    private void Load()
    {
        LoadFromJsonFile();
        StartCoroutine(MovePlayer());
    }

    //TODO: Add this later on
    IEnumerator MovePlayer()
    {
        WaitForSeconds timeToWait = new WaitForSeconds(1.0f / capturesPerSecond);

        for (int i = 0; i < playerData.positions.Count; i++)
        {
            player.SetPosition(playerData.positions[i]);
            player.SetLookDirection(playerData.lookDirections[i]);
            yield return timeToWait;
        }
    }

    private void LoadFromJsonFile()
    {
        //Load
        jsonFile = File.ReadAllText(Directory.GetFiles(JsonPath)[0]);
        playerData = JsonUtility.FromJson<DataHolder>(jsonFile);
    }
}
