using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] private List<SaveDataBaseClass> saveDatas = new List<SaveDataBaseClass>();
    [SerializeField] float capturesPerSecond = 5.0f;
    [SerializeField]private bool capturingData = true;

    private string jsonFile;

    public string JsonPath { get; private set; }
    public string JsonPrefix { get; private set; }

    private DataHolder playerData;

    private void Awake()
    {
        JsonPath = Directory.CreateDirectory(Application.dataPath + "/DebugStats/" + SceneManager.GetActiveScene().name + "/").FullName;
        //Debug.Log(JsonPath);
        JsonPrefix = "_" + SceneManager.GetActiveScene().name + "_" + Random.Range(0,10000000) + "_debugStats.json";

        if (capturingData == true)
        {
            StartCoroutine(AddDataTimer());
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
        if (capturingData == true)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                OnSave();
            }
        }
    }

    private void OnAddData()
    {
        foreach (var save in saveDatas)
        {
            save.OnAddData();
        }
    }

    public void OnSave()
    {
        if (capturingData == true)
        {
            foreach (var save in saveDatas)
            {
                save.OnSave();
            }
            Debug.Log("Saved Json!");
            capturingData = false;
        }
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
}
