using UnityEngine;
using UnityEditor;

public class EditDialogLineWindow : EditorWindow {

    // private static void ShowWindow() 
    // {
    //     var window = GetWindow<EditDialogLineWindow>();
    //     window.titleContent = new GUIContent("EditDialogLineWindow");
    //     window.Show();
    // }
    public int stringIndex;
    public string stringContent;
    public Sprite image;
    public string id;
    public DialogScriptableObject dialog;
    public bool confirm;
    
    private void OnGUI() 
    {
        titleContent = new GUIContent("Edit Dialog Line");

        GUILayout.Label("Character Name");

        EditorGUILayout.TextField(id);

        GUILayout.Label("Content");
        stringContent = EditorGUILayout.TextArea(stringContent, GUILayout.ExpandHeight(true));

        GUILayout.Space(25f);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("OK"))
        {
            confirm = true;
            dialog.strings[stringIndex].sentence = stringContent;
            Close();
        }

        if (GUILayout.Button("Cancel"))
        {
            Close();
        }

        GUILayout.EndHorizontal();
    }
}