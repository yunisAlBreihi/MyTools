using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

#if(UNITY_EDITOR)
public class ShowPlayerDataTool : EditorWindow
{
    [SerializeField] private ReplayDataScriptable replayData;

    private List<DebugLine> debugLines = new List<DebugLine>();
    private List<DataHolder> dataHolders = new List<DataHolder>();
    private List<string> dataNames = new List<string>();
    private List<float> lineDistances = new List<float>();
    private List<string> jsonFiles = new List<string>();
    private DataHolder currentData = null;
    private GameObject debugLinePrefab = null;
    private GameObject cameraPrefab = null;
    private GameObject debugCamera = null;
    private Vector3 cameraOffset = Vector3.zero;
    private string jsonPath = "";

    private int positionDelta = 0;
    private int popUpIndex = 0;

    private Rect buttonRect = new Rect(10, 10, 100, 20);
    private Rect popUpRect = new Rect(10, 30, 100, 10);
    private Rect popUpButtonRect = new Rect(130, 29, 100, 20);
    private Rect sliderRect = new Rect(10, 60, 500, 20);
    private Rect replayDataRect = new Rect(10, 100, 200, 20);
    private Rect replayButtonRect = new Rect(220, 100, 100, 20);

    static ShowPlayerDataTool window = null;

    [MenuItem("Tools/ShowPlayerData")]
    static void Init()
    {
        window = (ShowPlayerDataTool)EditorWindow.GetWindow(typeof(ShowPlayerDataTool));
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
        DestroyDebugObjects();
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

        if (dataHolders.Count != 0)
        {
            popUpIndex = EditorGUI.Popup(popUpRect, popUpIndex, dataNames.ToArray());

            if (GUI.Button(popUpButtonRect, "Select Data"))
            {
                SelectData();
            }

            replayData = (ReplayDataScriptable)EditorGUI.ObjectField(replayDataRect, replayData, typeof(ReplayDataScriptable));
            if (GUI.Button(replayButtonRect, "Replay Data"))
            {
                if (replayData != null)
                {
                    replayData.Clear();
                    StartPlay();
                }
                else
                {
                    Debug.LogWarning("Select ReplayData first!");
                }
            }
        }

        if (currentData != null)
        {
            positionDelta = EditorGUI.IntSlider(sliderRect, positionDelta, 0, currentData.positions.Count - 1);
            if (positionDelta >= currentData.positions.Count - 1)
            {
                positionDelta = currentData.positions.Count - 1;
            }
        }

        if (GUI.changed)
        {
            MoveCamera(positionDelta);
            Selection.activeObject = debugCamera;
        }
    }

    private void DestroyDebugObjects() 
    {
        //Destroy the used objects when closing the window
        foreach (var debugLine in debugLines)
        {
            DestroyImmediate(debugLine.gameObject);
        }
        DestroyImmediate(debugCamera);

        Resources.UnloadUnusedAssets();
    }

    private void StartPlay()
    {
        replayData.StartReplay(currentData);
        DestroyDebugObjects();
        window.Close();
        EditorApplication.isPlaying = true;
    }

    private void SelectData()
    {
        currentData = dataHolders[popUpIndex];
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
        int nameIndex = 0;

        for (int i = 0; i < dataFiles.Length; i++)
        {
            if (dataFiles[i].EndsWith(".json"))
            {
                nameIndex++;
                jsonFiles.Add(dataFiles[i]);
                dataNames.Add("Data" + nameIndex);
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
#endif