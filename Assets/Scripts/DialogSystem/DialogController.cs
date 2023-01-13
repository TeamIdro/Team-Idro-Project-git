using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public enum DialagoStatus { Init, Typing, EndOfSentence, EndOfDialog }
public class DialogController : MonoBehaviour
{
    private int selectedDialogIndex;
    private string[] dialogsAssetsFound;

    public TextMeshProUGUI dialogTextObj;

    private char[] charText;
    public int index = 0;
    public float typingSpeed;
    public string previusId;

    public GameObject setPointObj;

    //DIALOG ASSET
    public DialogScriptableObject dialogAsset;

    //DICTIONARY
    Dictionary<string, GameObject> speakerList =
    new Dictionary<string, GameObject>();

    public Dictionary<string, GameObject> sortedSpeakerList = 
        new Dictionary<string, GameObject>();

    //COROUTINE 
    IEnumerator coroutine;

    //DIALOG STATUS
    public DialagoStatus dialogStatus;

    //INSTANCE
    private static DialogController _instance;

    public static DialogController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        dialogStatus = DialagoStatus.Init;
        // DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        coroutine = Type();
    }

    public IEnumerator Type()
    {
        dialogStatus = DialagoStatus.Typing;

        charText = dialogAsset.strings[index].sentence.ToCharArray();

        for (int i = 0; i < charText.Length; i++)
        {
            dialogTextObj.text += charText[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        EndOfDialogCheck();
    }    

    public void AddSpeaker()
    {
        NPCScript[] speakers = GameObject.FindObjectsOfType<NPCScript>();

        //ADD PLAYER BEFORE NPCS
        PlayerDialogScript ps = GameObject.FindObjectOfType<PlayerDialogScript>();

        if (ps != null 
            && ps.gameObject.GetComponent<PlayerInput>().isActiveAndEnabled)
        {
            speakerList.Add("Player", GameObject.FindObjectOfType<PlayerDialogScript>().gameObject);
        }
        
        foreach (NPCScript s in speakers)
        {
            speakerList.Add(s.id, s.gameObject);
        }

        foreach (KeyValuePair<string, GameObject> entry in speakerList.OrderBy(x => x.Key))
        {
            sortedSpeakerList.Add(entry.Key, entry.Value);
        }

        //Previus ID first save
        if (dialogAsset != null)
        {
            previusId = dialogAsset.strings[index].id;
        }

        PrintSpeakers();
    }

    public void PrintSpeakers()
    {
        foreach (KeyValuePair<string, GameObject> entry in speakerList)
        {
            Debug.Log("normal list - " + entry);
        }

        foreach (KeyValuePair<string, GameObject> entry in sortedSpeakerList)
        {
            Debug.Log("ordered list" + entry);
        }
    }

    public void NextSentence()
    {
        if(dialogAsset == null)
        {
            return;
        }

        coroutine = Type();

        if (index < dialogAsset.strings.Count 
                && dialogStatus != DialagoStatus.EndOfDialog 
                && (dialogStatus == DialagoStatus.EndOfSentence || dialogStatus == DialagoStatus.Init))
        {
                SetCanvasToActivate();

                SetTextMeshObj(dialogAsset.strings[index].colorText);

                SetPortraitImage(dialogAsset.strings[index].portrait);

                StartCoroutine(coroutine);
        }
        else if (dialogStatus == DialagoStatus.EndOfDialog)
        {
            DeactivateCanvas(sortedSpeakerList[dialogAsset.strings[index].id]);

            //SwitchControlAtEnd();

            Reset();
        }
    }

    private void SetPortraitImage(Sprite portrait)
    {
        Image[] imageList;
        imageList = sortedSpeakerList[dialogAsset.strings[index].id].gameObject.GetComponentsInChildren<Image>();

        foreach(Image i in imageList)
        {
            // Debug.Log("Image name: " + i.name);

            if(i.name == "Portrait")
            {
                i.sprite = portrait;
            }
        }

        
        // dialogTextObj.color = colorText;
    }

    //SCEGLIE IL CANVAS DA DISATTIVARE E QUELLO DA ATTIVARE 
    private void SetCanvasToActivate()
    {
        if (previusId != dialogAsset.strings[index].id)
        {
            DeactivateCanvas(sortedSpeakerList[previusId]);
            previusId = dialogAsset.strings[index].id;
        }

        ActivateCanvas(sortedSpeakerList[dialogAsset.strings[index].id], dialogAsset.strings[index].colorFrame);
    }

    //ATTIVA IL CANCAS DELLO SPEAKER E IMPOSTA IL COLORE DEL FRAME
    private void ActivateCanvas(GameObject gameObject, Color colorFrame) 
    {
        DialogBox canvas = gameObject.GetComponentInChildren<DialogBox>();

        canvas.canvas.SetActive(true);

        Image sp = canvas.gameObject.GetComponentInChildren<Image>();

        sp.color = colorFrame;

    }

    //DISATTIVA IL CANVAS DELLO SPEAKER
    private void DeactivateCanvas(GameObject gameObject) 
    {
        DialogBox canvas = gameObject.GetComponentInChildren<DialogBox>();

        canvas.canvas.SetActive(false);
    }

    private void SetTextMeshObj(Color colorText)
    {
        dialogTextObj =
            sortedSpeakerList[dialogAsset.strings[index].id].gameObject.GetComponentInChildren<TextMeshProUGUI>();

        dialogTextObj.text = "";
        dialogTextObj.color = colorText;
    }

    private void EndOfDialogCheck()
    {
        if (index < dialogAsset.strings.Count - 1)
        {
            index++;
            dialogStatus = DialagoStatus.EndOfSentence;
        }
        else
        {
            dialogStatus = DialagoStatus.EndOfDialog;            
        }
    }

    public void Reset()
    {
        index = 0;
        dialogStatus = DialagoStatus.Init;

        speakerList.Clear();
        sortedSpeakerList.Clear();

        dialogAsset = null;
        setPointObj = null;

        //SwitchToMinigameCam();

        //ChangeScene();

        //ActiveAllEvents();
    }
}
