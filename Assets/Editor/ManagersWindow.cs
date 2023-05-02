using Codice.CM.SEIDInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class ManagersWindow : ExtendedEditorWindow
{
    [MenuItem("Managers Data/Manager Window")]
    public static void OpenWindow()
    {
        const int width = 620;
        const int height = 620;

        var x = (Screen.currentResolution.width - width) / 2;
        var y = (Screen.currentResolution.height - height) / 2;
        ManagersWindow window = GetWindow<ManagersWindow>("Managers Data");
        window.position = new Rect(y, x, width, height);    


    }

    private string[] scriptableObjectAsset;
    Manager[] manager;
    private Editor cacheEditor;
    Vector2 scrollPos;
    private string m_scriptbleObjectToSearch;

    private void OnEnable()
    {
        cacheEditor = null;

    }
    private void OnGUI()
    {
       
        EditorGUILayout.BeginHorizontal(GUILayout.Width(200));

        EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
        scriptableObjectAsset = AssetDatabase.FindAssets("t:Manager");
        manager = Resources.FindObjectsOfTypeAll<Manager>();


        string[] scriptbleObjectName = new string[scriptableObjectAsset.Length];
        EditorGUILayout.LabelField("Choose the manager:", EditorStyles.whiteLargeLabel);



        for (int i = 0; i < manager.Length; i++)
        {
                   
            if (GUILayout.Button(manager[i].name, GUILayout.Width(200f), GUILayout.Height(50f)))
            {
                cacheEditor = Editor.CreateEditor(manager[i]);
            }

        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400), GUILayout.Height(400));
        if (cacheEditor != null) cacheEditor.OnInspectorGUI();
        
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

   
}
