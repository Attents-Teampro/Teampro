using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(DungeonCreator))]

public class DungeonCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DungeonCreator dungeonCreator = (DungeonCreator)target;
        if(GUILayout.Button("CreateNewDungeon"))
        {
            dungeonCreator.CreateDungeon();
        }
    }
}
#endif
