using System;
using UnityEditor;
using UnityEngine;

public class PixelMapEditorWindow : EditorWindow
{
    [MenuItem("Pixel Map/Tool Windows/Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PixelMapEditorWindow));
    }

    private Texture2D _mapImage;

    [System.Serializable]
    public struct Mappings
    {
        public GameObject spawnObj;
        public Color spawnColour;
    }

    public Mappings[] mappedElement;
    private Color _pixelColour;
    private bool test = true;

    private void OnGUI()
    {
        GUILayout.Label("Generate Map", EditorStyles.boldLabel);
        
        _mapImage = EditorGUILayout.ObjectField(("Map Image"), _mapImage, typeof(Texture2D), false) as Texture2D;
        mappedElement = EditorGUILayout.
    }
}
