using System;
using UnityEditor;
using UnityEngine;
using Unity.Mathematics;

public class PixelMapEditorWindow : EditorWindow
{
    [MenuItem("Window/Pixel Map/Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PixelMapEditorWindow));
    }

    private Texture2D _mapImage;
    private GameObject _mapParent;

    [System.Serializable]
    public struct Mappings
    {
        public GameObject spawnObj;
        public Color spawnColour;
    }

    private int _mappedElementSize;
    public Mappings[] mappedElement;
    private Color _pixelColour;
    private bool test = true;

    private void OnGUI()
    {
        GUILayout.Label("Generate Map", EditorStyles.boldLabel);

        _mapImage = EditorGUILayout.ObjectField(("Map Image"), _mapImage, typeof(Texture2D), false) as Texture2D;
        _mapParent = EditorGUILayout.ObjectField("Map Parent GameObject", _mapParent, typeof(GameObject), true) as GameObject;

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty mappedElementProperty = so.FindProperty("mappedElement");
        EditorGUILayout.PropertyField(mappedElementProperty, true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Generate Map Data"))
        {
            GenerateMapData();
        }
        if(_mapParent != null && _mapParent.transform.childCount > 0)
        {
            if(GUILayout.Button("Clear Parent Map Data"))
            {
                for(int i = _mapParent.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(_mapParent.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    private void GenerateMapData()
    {
        if (_mapImage == null) { Debug.LogError("Map Image is null. Please select a Texture2D image."); }
        else if(_mapParent == null) { Debug.LogError("Map Parent Gameobject has not been defined"); }
        else if(_mapParent != null && _mapParent.transform.childCount > 0) { Debug.LogError("Map data already exists within selected parent gameobject."); }
        else { GenerateLevel(); }
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
        //scan whole texture and get pixel positions
        for (int x = 0; x < _mapImage.width; x++)
        {
            for (int y = 0; y < _mapImage.height; y++)
            {
                GenerateObject(x, y);
            }
        }
    }
}
