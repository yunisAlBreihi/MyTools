using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MiniMapTool : EditorWindow
{
    private Rect miniMapRect = new Rect(900, 320, 230, 250);
    private Rect textureViewRect = new Rect(0, 20, 230, 230);
    private Rect checkMouseBodyRect;
    private Rect checkMouseRibbonRect;

    private RenderTexture miniMapTexture;
    private GameObject miniMapCameraObject;
    private Camera miniMapCamera;

    public RectTransform worldImage; //RawImage which shows RenderTexture from camera

    bool holdingRibbon = false;
    bool holdingMiniMap = false;

    [MenuItem("Tools/MiniMap")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MiniMapTool window = (MiniMapTool)EditorWindow.GetWindow(typeof(MiniMapTool));
        window.Show();
    }
    private void Awake()
    {
        miniMapCameraObject = new GameObject("MiniMapCamera");
        miniMapCameraObject.transform.Translate(0.0f, 80.0f, 0.0f);
        miniMapCameraObject.transform.Rotate(90.0f, 0.0f, 0.0f);
        miniMapCamera = miniMapCameraObject.AddComponent<Camera>();

        //Since the rect is created with the GUI top ribbon in mind,
        //we need to subtract the height of the ribbon to get correct values
        //When checking if the mouse is inside the minimap
        //Subtract the ribbon from the calculations
        checkMouseBodyRect = new Rect(
            miniMapRect.x,
            miniMapRect.y - GetGUIRibbonSize().y + (miniMapRect.height - miniMapRect.width),
            miniMapRect.width,
            miniMapRect.height - (miniMapRect.height - miniMapRect.width));

        //Make a Rect for the ribbon to be able to manipulate the window
        checkMouseRibbonRect = new Rect(
            miniMapRect.x,
            miniMapRect.y - GetGUIRibbonSize().y,
            miniMapRect.width,
            miniMapRect.height - checkMouseBodyRect.height);
    }

    private void OnDestroy()
    {
        DestroyImmediate(miniMapCameraObject);
    }

    private void OnGUI()
    {
        miniMapTexture = EditorGUILayout.ObjectField("RenderTexture: ", miniMapTexture,
                                                    typeof(RenderTexture), false) as RenderTexture;
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += SceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= SceneGUI;
    }

    private void SceneGUI(SceneView sceneView)
    {

        ////This fixes the EventType.MouseUp not working with the LMB
        //if (Event.current.type == EventType.Layout)
        //    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));

        //Creates MiniMap window and updates the texture
        miniMapRect = GUI.Window(0, miniMapRect, DoMyWindow, "MiniMap");

        //turns on hold MiniMap if pressed LMB on the minimap
        if (checkMouseBodyRect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0)
                {
                    holdingMiniMap = true;
                }
            }
        }
        else
        {
            holdingMiniMap = false;
        }
        //turns on hold Ribbon if pressed LMB on the minimap ribbon
        if (checkMouseRibbonRect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown)
            {
                sceneView.pivot += Vector3.right;
                sceneView.pivot += Vector3.left;
                if (Event.current.button == 0)
                {
                    holdingRibbon = true;
                }
            }
        }

        if (holdingMiniMap == true)
        {
            sceneView.pivot = GetLocationOnMap();
            Debug.Log(sceneView.pivot);
        }

        if (holdingRibbon == true)
        {
            Debug.Log("Ribbon");
        }

        if (Event.current.type == EventType.MouseUp)
        {
            if (Event.current.button == 0)
            {
                Debug.Log("MouseUp");
                holdingMiniMap = false;
                holdingRibbon = false;
            }
        }
    }

    void SetMiniMapWindowPosition(Vector2 position)
    {
        Vector2 miniMapRectOffset = position - miniMapRect.position;
        miniMapRect.position += miniMapRectOffset;
        Vector2 textureViewRectOffset = position - textureViewRect.position;
        textureViewRect.position += textureViewRectOffset;
        Vector2 checkMouseRibbonRectOffset = position - checkMouseRibbonRect.position;
        checkMouseRibbonRect.position += checkMouseRibbonRectOffset;
        miniMapRect = GUI.Window(0, miniMapRect, DoMyWindow, "MiniMap");
    }

    void DoMyWindow(int windowID)
    {
        if (miniMapTexture != null && miniMapCameraObject != null)
        {
            miniMapCamera.targetTexture = miniMapTexture;
            GUI.DrawTexture(textureViewRect, miniMapTexture);
        }
    }

    Vector2 GetGUIRibbonSize()
    {
        GUIStyle ribbonStyle = "GV Gizmo DropDown";
        Vector2 ribbonSize = ribbonStyle.CalcSize(SceneView.lastActiveSceneView.titleContent);

        return ribbonSize;
    }

    Vector3 GetLocationOnMap()
    {
        Vector2 MousePosInMiniMap = Event.current.mousePosition - checkMouseBodyRect.position;
        Vector2 screenPositionPercent = MousePosInMiniMap / checkMouseBodyRect.size;
        Vector2 positionOnMiniMapCamera = new Vector2(miniMapCamera.scaledPixelWidth, miniMapCamera.scaledPixelHeight) * screenPositionPercent;
        Vector3 MiniCameraWorldPos = miniMapCamera.ScreenToWorldPoint(new Vector3(positionOnMiniMapCamera.x, positionOnMiniMapCamera.y, miniMapCameraObject.transform.position.y));
        Vector3 invertedWorldPos = new Vector3(MiniCameraWorldPos.x, MiniCameraWorldPos.y, MiniCameraWorldPos.z * -1.0f);
        return invertedWorldPos;
    }
}
