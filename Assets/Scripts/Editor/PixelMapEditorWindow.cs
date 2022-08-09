using System;
using UnityEditor;
using UnityEngine;
using Unity.Mathematics;
using System.IO;

public class PixelMapEditorWindow : EditorWindow
{
    [MenuItem("Window/Pixel Map/Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PixelMapEditorWindow));
    }

    private Texture2D _mapImage;
    private GameObject _mapParent;
    private string _mapName;

    [System.Serializable]
    public struct Mappings
    {
        public GameObject spawnObj;
        public Color spawnColour;
    }

    public Mappings[] mappedElement;
    private Color _pixelColour;

    private string _pathDirectory = "Assets/Resources/MapData/";

    private void OnGUI()
    {
        GUILayout.Label("Generate Map", EditorStyles.boldLabel);

        _mapImage = EditorGUILayout.ObjectField(new GUIContent("Map Image", "The image containing colour data for the tool to create the map."), _mapImage, typeof(Texture2D), false) as Texture2D;
        _mapName = EditorGUILayout.TextField(new GUIContent("Map Name", "The name for the parent GameObject containing the created map data."), _mapName);

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty mappedElementProperty = so.FindProperty("mappedElement");
        EditorGUILayout.PropertyField(mappedElementProperty, true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Generate Map Data"))
        {
            GenerateMapData();
        }
        if (_mapParent != null && _mapParent.transform.childCount > 0)
        {
            if (GUILayout.Button("Generate Prefab From MapData"))
            {
                GeneratePrefab();
            }
            if (GUILayout.Button("Clear Map Data"))
            {
                DestroyImmediate(_mapParent);
            }
        }
    }

    private void GenerateMapData()
    {
        if (_mapImage == null) { Debug.LogError("Map Image is null. Please select a Texture2D image."); }
        else if(_mapName == string.Empty) { Debug.LogError("Please enter a name for the map."); }
        else if(_mapParent != null && _mapParent.transform.childCount > 0) { Debug.LogError("Map data already exists within selected parent gameobject."); }
        else 
        { 
            GenerateLevel();
        }
    }

    void GenerateObject(int x, int y)
    {
        
        //read pixel colours
        _pixelColour = _mapImage.GetPixel(x, y);
        if (_pixelColour.a == 0)
        {
            return;
        }

        foreach (Mappings colourMapping in mappedElement)
        {
            //Scan pixel colour mappings for matching colour
            if (colourMapping.spawnColour.Equals(_pixelColour))
            {
                //turn the pixel x and y into Vector2 position
                Vector2 pos = new Vector2(x, y);
                //Spawn object that matches pixel colour at pixel position
                Instantiate(colourMapping.spawnObj, pos, quaternion.identity, _mapParent.transform);
            }
        }
    }

    void GenerateLevel()
    {
        _mapParent = new GameObject(_mapName);
        //scan whole texture and get pixel positions
        for (int x = 0; x < _mapImage.width; x++)
        {
            for (int y = 0; y < _mapImage.height; y++)
            {
                GenerateObject(x, y);
            }
        }
    }

    void GeneratePrefab()
    {
        if (Directory.Exists(_pathDirectory))
        {
            string localPath = _pathDirectory + _mapParent.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(_mapParent, localPath);
            AssetDatabase.Refresh();
        }
        else
        {
            Directory.CreateDirectory(_pathDirectory);
            AssetDatabase.Refresh();
        }
    }
}
