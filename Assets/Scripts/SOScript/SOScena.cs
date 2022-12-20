using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName ="SceneSO",menuName ="SceneSO")]

public class SOScena : ScriptableObject
{
    public SceneReference sceneReference;
    public GameStates currentStateInScene;
}
