using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActivator : MonoBehaviour
{
    public DialogScriptableObject asset;

    void Start()
    {
        DialogController.Instance.dialogAsset = asset;
        DialogController.Instance.AddSpeaker();
        DialogController.Instance.NextSentence();
    }

}
