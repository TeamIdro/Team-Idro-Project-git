using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
        DontDestroyOnLoad(this.gameObject);
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

        // EndOfDialogCheck();
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

            }
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
}
