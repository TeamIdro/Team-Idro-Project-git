using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Interaction 
{ 

}

public enum BubblePosition { Right, Left}

public class DialogScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class DialogString
    {
        public string sentence;
        public int id;
        public Interaction interaction;
        public Color colorFrame;
        public Color colorText;
        public BubblePosition bubblePosition;
        public Sprite portrait;
    }

    public List<DialogString> strings = new List<DialogString>();

}
