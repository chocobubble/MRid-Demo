using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(SaveLoadMap))]
public class SaveLoadMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveLoadMap saveLoadMap = (SaveLoadMap) target;
        if(GUILayout.Button("save"))
        {
            Debug.Log("Saving map");
            saveLoadMap.Save();
        }
    }
}

