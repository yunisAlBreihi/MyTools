using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public abstract class SaveDataBaseClass : MonoBehaviour
{
    [SerializeField] private SaveHandler saveHandler = null;
    [SerializeField] private bool uploadData = true;

    public DataHolder data = null;

    private FTPUploader uploader = new FTPUploader();

    protected virtual void Awake()
    {
        if (data == null)
        {
            Debug.LogError("You need to set a data scriptable object!");
        }
    }

    protected virtual void Start()
    {
        SaveDataBaseClass[] saveDataComponents = gameObject.GetComponents<SaveDataBaseClass>();
        foreach (var dataComponent in saveDataComponents)
        {
            if (dataComponent.data != null)
            {
                data = dataComponent.data;
                break;
            }
        }
        if (data == null)
        {
            data = new DataHolder();
        }

        if (saveHandler == null)
        {
            saveHandler = FindObjectOfType<SaveHandler>();
        }
        saveHandler.AddSaveData(this);
    }

    public virtual void OnSave() 
    {
        OnAddData();
        string jsonFileName = gameObject.name + saveHandler.JsonPrefix;
        string jsonFile = JsonUtility.ToJson(data);
        File.WriteAllText(saveHandler.JsonPath + jsonFileName, jsonFile);
        uploader.UploadFile(saveHandler.JsonPath + jsonFileName);
    }

    public virtual void OnLoad()
    {
    }

    public abstract void OnAddData();
}
