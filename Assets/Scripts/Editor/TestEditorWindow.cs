using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;

//creates a script that derives from EditorWindow
public class TestEditorWindow : EditorWindow
{
    //This is the menu, sub menu and window names
    [MenuItem("Window/Rubber Duck/Test Window")]
    //This public static function is what creates the window
    public static void ShowWindow()
    {
        //GetWindow comes from EditorWindow library
        GetWindow(typeof(TestEditorWindow));
    }

    private string _objectBaseName = "";
    private string _objectTag = "";
    private GameObject _objectToSpawn;
    private float _objectScale = 1;
    private Transform _sceneParent;
    private Positions _position;
    private int _spawnIndex = 0;
    private Vector3 _spawnPosition;

    public enum Positions
    {
        Disconnected,
        ParentPosition,
        Connected,
        Default
    }
    
    private void OnGUI()
    {
        //The display of the actual window
        GUILayout.Label("Spawn New Object", EditorStyles.boldLabel);
        
        _objectToSpawn = EditorGUILayout.ObjectField
            ("Prefab To Spawn", _objectToSpawn, typeof(GameObject), false) as GameObject;
        if (_objectToSpawn != null) //if _objectToSpawn is not null
        {
            //shows EditorWindow TextField with label ObjectName
            _objectBaseName = EditorGUILayout.TextField("Object Name", _objectBaseName);
            //shows EditorWindow TagField with label ObjectTag
            _objectTag = EditorGUILayout.TagField("Object Tag", _objectTag);
            //shows EditorWindow Slider with label ObjectScale
            _objectScale = EditorGUILayout.Slider("Object Scale", _objectScale, 0.25f, 10f);
            //shows EditorWindow ObjectField with label SceneParent
            _sceneParent = EditorGUILayout.ObjectField("Scene Parent", _sceneParent, typeof(Transform), true) as Transform;
            _spawnIndex = EditorGUILayout.Popup("Spawn Option", _spawnIndex, Enum.GetNames(typeof(Positions)));
            if (_spawnIndex == 0 || _spawnIndex == 2)
            {
                _spawnPosition = EditorGUILayout.Vector3Field("Spawn Position", _spawnPosition);
            }
            if (GUILayout.Button("Spawn Object"))//if user presses SpawnObject button
            {
                SpawnObject();//runs SpawnObject function
            }

            
        }

        if (_sceneParent.childCount > 0)
        {
            if (GUILayout.Button("Remove All Objects"))
            {
                /*foreach (Transform child in _sceneParent.transform)
                {
                    Destroy(child.gameObject);
                }*/

                for (int i = _sceneParent.childCount - 1; i > 0; i--)
                {
                    DestroyImmediate(_sceneParent.GetChild(i).gameObject);
                }
            }
        }
    }

    private void SpawnObject()
    {
        if (_objectBaseName == string.Empty)//if _objectBaseName is not filled in
        {
            Debug.LogError("Error: Please Enter A Name For This Object");//displays a console error
        }

        GameObject newObject = Instantiate(_objectToSpawn);
        //GameObject newObject = Instantiate(_objectToSpawn, Vector3.zero, quaternion.identity, _sceneParent);
        switch (_spawnIndex)
        {
            case 0:
                newObject.transform.position = _spawnPosition;
                newObject.transform.localScale = Vector3.one * _objectScale;
                newObject.name = _objectBaseName;
                newObject.tag = _objectTag;
                break;
            case 1:
                newObject.transform.position = _sceneParent.position;
                newObject.transform.localScale = Vector3.one * _objectScale;
                newObject.name = _objectBaseName;
                newObject.tag = _objectTag;
                break;
            case 2:
                newObject.transform.position = _sceneParent.position + _spawnPosition;
                newObject.transform.parent = _sceneParent;
                newObject.transform.localScale = Vector3.one * _objectScale;
                newObject.name = _objectBaseName;
                newObject.tag = _objectTag;
                break;
            case 3:
                newObject.transform.position = Vector3.zero;
                newObject.transform.localScale = Vector3.one * _objectScale;
                newObject.name = _objectBaseName;
                newObject.tag = _objectTag;
                break;
        }
        //Creates a GameObject newObject and instantiates it
        //GameObject newObject = Instantiate(_objectToSpawn, Vector3.zero, quaternion.identity, _sceneParent);
        //applies localScale, name and tag based on EditorWindow information
        
    }
}
