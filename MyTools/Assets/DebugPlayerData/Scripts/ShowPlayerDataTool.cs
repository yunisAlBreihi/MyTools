using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ShowPlayerDataTool : EditorWindow
{
    private List<DebugLine> debugLines = new List<DebugLine>();
    private List<DataHolder> dataHolders = new List<DataHolder>();
    private List<float> lineDistances = new List<float>();
    private List<string> jsonFiles = new List<string>();
    private DataHolder currentData = null;
    private GameObject debugLinePrefab = null;
    private GameObject cameraPrefab = null;
    private GameObject debugCamera = null;
    private Vector3 cameraOffset = Vector3.zero;
    private string jsonPath = "";

    private int positionDelta = 0;

    private Rect sliderRect = new Rect(10, 50, 500, 20);
    private Rect buttonRect = new Rect(10, 10, 100, 20);

    [MenuItem("Tools/ShowPlayerData")]
    static void Init()
    {
        ShowPlayerDataTool window = (ShowPlayerDataTool)EditorWindow.GetWindow(typeof(ShowPlayerDataTool));
        window.Show();
    }

    private void OnEnable()
    {
        debugLinePrefab = (GameObject)Resources.Load("DebugLine", typeof(GameObject));
        cameraPrefab = (GameObject)Resources.Load("DebugLineCamera", typeof(GameObject));

        SceneView.duringSceneGui += SceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= SceneGUI;

        //Destroy the used objects when closing the window
        foreach (var debugLine in debugLines)
        {
            DestroyImmediate(debugLine.gameObject);
        }
        DestroyImmediate(debugCamera);

        Resources.UnloadUnusedAssets();
    }

    private void OnGUI()
    {
        if (GUI.Button(buttonRect, "Load Path"))
        {
            GetDebugFiles();

            CreateDebugLines();

            CreateCamera();


            //for (int i = 0; i < dataHolders.Count; i++)
            //{
            //    for (int j = 0; j < dataHolders[i].positions.Count; j++)
            //    {
            //        if (i + 1 < dataHolders[i].positions.Count)
            //        {
            //            lineDistances[i] += Vector3.Distance(dataHolders[i].positions[j], dataHolders[i].positions[j + 1]);
            //        }
            //    }
            //}

            //TODO: add menu for choosing files
        }

        if (currentData != null)
        {
            positionDelta = EditorGUI.IntSlider(sliderRect, positionDelta, 0, currentData.positions.Count - 1);
        }

        if (GUI.changed)
        {
            MoveCamera(positionDelta);
            Selection.activeObject = debugCamera;
        }
    }

    private void CreateCamera()
    {
        debugCamera = Instantiate(cameraPrefab);
        cameraOffset = debugCamera.transform.position;
        MoveCamera(0);
    }

    private void MoveCamera(int positionIndex)
    {
        debugCamera.transform.position = currentData.positions[positionIndex] + cameraOffset;
        debugCamera.transform.rotation = Quaternion.LookRotation(currentData.lookDirections[positionIndex]);
    }

    private void GetDebugFiles()
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
        string currentFile = File.ReadAllText(jsonFile);
        currentData = JsonUtility.FromJson<DataHolder>(currentFile);
        dataHolders.Add(currentData);
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
