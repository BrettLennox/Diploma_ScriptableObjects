using System;
using UnityEditor;
using UnityEngine;
using System.IO;

public class PixelMapEditorWindow : EditorWindow
{
    [MenuItem("Window/Pixel Map/Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PixelMapEditorWindow));
    }

    private Texture2D _mapImage; // The Image that displays the maps colours
    private GameObject _mapParent; //The parent GameObject that holds the generated map data
    private string _mapName; //The name of the mapParent that is created

    [System.Serializable]
    public struct Mappings //Struct containing the object to spawn and the colour to match them with
    {
        public GameObject spawnObj;
        public Color spawnColour;
    }

    public Mappings[] mappedElement; //Array for the Mapping data
    private Color _pixelColour; //Color for checking the map image

    private string _pathDirectory = "Assets/Resources/MapData/"; //string reference to the PathDirectory to save the prefab to

    private SerializedObject sObj; //SerializedObject for displaying the array in the Editor

    private void OnEnable()
    {
        sObj = new SerializedObject(this); //sets the sObj to this script which inherits from EditorWindow which inherits from ScriptableObject
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate Map", EditorStyles.boldLabel); //Creates a label at the top of the EditorWindow with set information

        //Creates an ObjectField in the EditorWindow that contains Texture2D information and updates _mapImage to it
        _mapImage = EditorGUILayout.ObjectField(new GUIContent("Map Image", "The image containing colour data for the tool to create the map."), _mapImage, typeof(Texture2D), false) as Texture2D;
        //Creates a TextField in the EditorWindow that contains string information and updates _mapName to it
        _mapName = EditorGUILayout.TextField(new GUIContent("Map Name", "The name for the parent GameObject containing the created map data."), _mapName);

        //Creates a PropertyField in the EditorWindow that displays the mappedElement array information
        EditorGUILayout.PropertyField(sObj.FindProperty("mappedElement"), true);
        sObj.ApplyModifiedProperties(); //Applies the updated information in the array PropertyField to the SerializedObject

        if (GUILayout.Button("Generate Map Data")) //If the user presses the GenerateMapData button
        {
            GenerateMapData();//Run this function
        }
        if (_mapParent != null && _mapParent.transform.childCount > 0) //If mapParent is not null and contains children
        {
            if (GUILayout.Button("Generate Prefab From MapData")) //If the user presses GeneratePrefabFromMapData button
            {
                GeneratePrefab(); //Run this function
            }
            if (GUILayout.Button("Clear Map Data")) //If the user presses the ClearMapData button
            {
                DestroyImmediate(_mapParent); //Destroy _mapParent GameObject
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
                Instantiate(colourMapping.spawnObj, pos, Quaternion.identity, _mapParent.transform);
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
