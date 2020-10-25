using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ShowPlayerDataTool : EditorWindow
{
    private List<DebugLine> debugLines = new List<DebugLine>();
    private List<string> jsonFiles = new List<string>();
    private DataHolder currentData = null;
    private GameObject debugLinePrefab = null;
    private string jsonPath = "";
    private string currentFile = "";

    private Rect buttonRect = new Rect(0,0,100,20);

    [MenuItem("Tools/ShowPlayerData")]
    static void Init() 
    {
        ShowPlayerDataTool window = (ShowPlayerDataTool)EditorWindow.GetWindow(typeof(ShowPlayerDataTool));
        window.Show();
    }

    private void OnEnable()
    {
        debugLinePrefab = (GameObject)Resources.Load("DebugLine", typeof(GameObject));

        SceneView.duringSceneGui += SceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= SceneGUI;
        foreach (var debugLine in debugLines)
        {
            DestroyImmediate(debugLine.gameObject);
        }
        Resources.UnloadUnusedAssets();
    }

    private void OnGUI()
    {
        if (GUI.Button(buttonRect, "Load Path"))
        {
            jsonPath = EditorUtility.OpenFolderPanel("Choose DebugDeta Path", Application.dataPath, "");

            //Only add the files that end with .json
            string[] dataFiles = Directory.GetFiles(jsonPath);
            for (int i = 0; i < dataFiles.Length; i++)
            {
                if (dataFiles[i].EndsWith(".json"))
                {
                    jsonFiles.Add(dataFiles[i]);
                }
            }

            CreateDebugLines();
            //Debug.Log(jsonFile);
            //LoadJsonFile(jsonFiles[0]);

            //TODO: add menu for choosing files
        }
    }

    private void CreateDebugLines() 
    {
        foreach (var file in jsonFiles)
        {
            LoadJsonFile(file);
            CreateSingleLine();
        }
    }

    private void CreateSingleLine() 
    {
        GameObject gameObject = Instantiate(debugLinePrefab);
        DebugLine line = gameObject.GetComponent<DebugLine>();
        if (line != null)
        {
            line.Create(currentData);
            debugLines.Add(line);
        }
    }

    private void LoadJsonFile(string jsonFile)
    {
        currentFile = File.ReadAllText(jsonFile);
        currentData = JsonUtility.FromJson<DataHolder>(currentFile);
    }

    private void OnMenuClick() 
    {
    
    }

    private void Awake()
    {

    }

    private void SceneGUI(SceneView sceneView)
    {
        //Handles.BeginGUI();

        //foreach (var file in jsonFiles)
        //{
        //    LoadJsonFile(file);
        //    CreateSingleLine();

        //    for (int i = 0; i < currentData.positions.Count; i++)
        //    {
        //        if (i + 1 < currentData.positions.Count)
        //        {
        //            Debug.DrawLine(currentData.positions[i], currentData.positions[i + 1]);
        //        }
        //    }
        //}

        //Handles.EndGUI();
    }
}
