using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Interaction { NONE }
public enum BubblePosition { Right, Left}

public class DialogScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class DialogString
    {
        public string sentence;
        public string id;
        public Interaction interaction;
        public Color colorFrame;
        public Color colorText;
        public Sprite portrait;
    }

    public List<DialogString> strings = new List<DialogString>();

}