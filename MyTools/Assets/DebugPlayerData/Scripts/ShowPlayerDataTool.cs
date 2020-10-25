using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ShowPlayerDataTool : EditorWindow
{
    public PlayerSave player;

    private Vector3 playerStartPosition;
    private Vector3 playerPosition;

    private PlayerData playerData;
    private string jsonFile;
    private string jsonPath;

    [MenuItem("Tools/ShowPlayerData")]
    static void Init() 
    {
        ShowPlayerDataTool window = (ShowPlayerDataTool)EditorWindow.GetWindow(typeof(ShowPlayerDataTool));
        window.Show();
    }

    private void OnEnable()
    {
        jsonPath = Application.dataPath + "/debugStats.json";
        player = FindObjectOfType<PlayerSave>();
        InitiatePlayer();

        SceneView.duringSceneGui += SceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= SceneGUI;
    }

    private void OnGUI()
    {
        player = (PlayerSave)EditorGUILayout.ObjectField(player, typeof(PlayerSave),true);

        if (GUI.changed == true)
        {
            InitiatePlayer();
        }
    }

    private void Awake()
    {

    }

    private void SceneGUI(SceneView sceneView) 
    {
        Handles.BeginGUI();

        if (playerData != null)
        {
            for (int i = 0; i < playerData.positions.Count; i++)
            {
                if (i + 1 < playerData.positions.Count)
                {
                    Debug.DrawLine(playerData.positions[i], playerData.positions[i + 1]);
                }
            }
        }

        Handles.EndGUI();
    }

    private void LoadJsonFile() 
    {
        jsonFile = File.ReadAllText(jsonPath);
        playerData = JsonUtility.FromJson<PlayerData>(jsonFile);
    }

    private void InitiatePlayer() 
    {
        LoadJsonFile();
        playerStartPosition = player.transform.position;
        playerPosition = playerStartPosition;
    }
}
