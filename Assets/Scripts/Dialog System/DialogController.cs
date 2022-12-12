// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using System.Linq;
// using UnityEngine.UI;
// using UnityEngine.InputSystem;

// public enum DialagoStatus { Init, Typing, EndOfSentence, EndOfDialog }

// public class DialogController : MonoBehaviour
// {
//     private int selectedDialogIndex;
//     private string[] dialogsAssetsFound;

//     public TextMeshProUGUI dialogTextObj;

//     private char[] charText;
//     public int index = 0;
//     public float typingSpeed;
//     public int previusId;

//     public GameObject setPointObj;

//     //DIALOG ASSET
//     public DialogScriptableObject dialogAsset;

//     //DICTIONARY
//     Dictionary<int, GameObject> speakerList =
//     new Dictionary<int, GameObject>();

//     public Dictionary<int, GameObject> sortedSpeakerList = 
//         new Dictionary<int, GameObject>();

//     //COROUTINE 
//     IEnumerator coroutine;

//     //DIALOG STATUS
//     public DialagoStatus dialogStatus;

//     //INSTANCE
//     private static DialogController _instance;

//     public static DialogController Instance
//     {
//         get
//         {
//             return _instance;
//         }
//     }

//     private void Awake()
//     {
//         _instance = this;
//         dialogStatus = DialagoStatus.Init;
//         DontDestroyOnLoad(this.gameObject);
//     }

//     private void Start()
//     {
//         coroutine = Type();
//     }

//     public void Reset()
//     {
//         index = 0;
//         dialogStatus = DialagoStatus.Init;

//         speakerList.Clear();
//         sortedSpeakerList.Clear();

//         dialogAsset = null;
//         setPointObj = null;

//         SwitchToMinigameCam();

//         ChangeScene();

//         ActiveAllEvents();
//     }

//     public IEnumerator Type()
//     {
//         dialogStatus = DialagoStatus.Typing;

//         charText = dialogAsset.strings[index].sentence.ToCharArray();

//         for (int i = 0; i < charText.Length; i++)
//         {
//             dialogTextObj.text += charText[i];
//             yield return new WaitForSeconds(typingSpeed);
//         }

//         EndOfDialogCheck();
//     }

//     //ALLA FINE DELLA FRASE SALTA INIZIA QUELLA SUCCESSIVA TRAMITE INPUT (E o (A - CONTROLLER))
//     public void NextSentence()
//     {
//         if (dialogAsset != null)
//         {
//             coroutine = Type();

//             if (index == 0 && setPointObj != null)
//             {
//                 SetPosition(setPointObj);
//             }

//             if (index < dialogAsset.strings.Count 
//                 && dialogStatus != DialagoStatus.EndOfDialog 
//                 && (dialogStatus == DialagoStatus.EndOfSentence || dialogStatus == DialagoStatus.Init))
//             {
//                 SetCanvasToActivate();

//                 SetTextMeshObj(dialogAsset.strings[index].colorText);

//                 //set animation
//                 //Debug.Log("CHANGE ANIMATION DC in " + dialogAsset.strings[index].interaction.ToString());
//                 if (sortedSpeakerList[dialogAsset.strings[index].id].gameObject.GetComponentInChildren<NPCScripts>())
//                 {
//                     sortedSpeakerList[dialogAsset.strings[index].id].gameObject.GetComponentInChildren<NPCScripts>().ChangeAnimation(dialogAsset.strings[index].interaction.ToString());
//                 }
//                 else
//                 {
//                     PlayerScript.Instance.GetComponent<PlayerMovementTest>().ChangeAnimation(dialogAsset.strings[index].interaction.ToString());
//                 }

//                 StartCoroutine(coroutine);
//             }
//             else if (dialogStatus == DialagoStatus.EndOfDialog)
//             {
//                 DeactivateCanvas(sortedSpeakerList[dialogAsset.strings[index].id]);

//                 SwitchControlAtEnd();

//                 Reset();
//             }
//         }
//     }

//     //SALTA ALLA LINEA DI DIALOGO SUCCESSIVA TRAMITE INPUT (Q o (O/B - TASTO DESTRO CONTROLLER))
//     public void SkipSentence()
//     {
//         if (dialogAsset != null)
//         {
//             StopCoroutine(coroutine);

//             if (dialogStatus != DialagoStatus.EndOfDialog)
//             {
//                 SetCanvasToActivate();

//                 SetTextMeshObj(dialogAsset.strings[index].colorText);
//             }

//             if (dialogStatus == DialagoStatus.Typing)
//             {
//                 dialogTextObj.text = dialogAsset.strings[index].sentence;

//                 dialogStatus = DialagoStatus.EndOfSentence;
//             }
//             else if (dialogStatus == DialagoStatus.EndOfSentence)
//             {
//                 dialogTextObj.text = dialogAsset.strings[index].sentence;
//             }

//             EndOfDialogCheck();
//         }
//     }

//     //AGGIUNGE GLI SPEAKER AL DICTIONARY E ORDINA PER KEY
//     public void AddSpeaker()
//     {
//         NPCScripts[] speakers = GameObject.FindObjectsOfType<NPCScripts>();

//         //ADD PLAYER BEFORE NPCS
//         PlayerScript ps = GameObject.FindObjectOfType<PlayerScript>();

//         if (ps != null 
//             && ps.gameObject.GetComponent<PlayerInput>().isActiveAndEnabled)
//         {
//             speakerList.Add(0, GameObject.FindObjectOfType<PlayerScript>().gameObject);
//         }
        
//         foreach (NPCScripts s in speakers)
//         {
//             speakerList.Add(s.id, s.gameObject);
//         }

//         foreach (KeyValuePair<int, GameObject> entry in speakerList.OrderBy(x => x.Key))
//         {
//             sortedSpeakerList.Add(entry.Key, entry.Value);
//         }

//         //Previus ID first save
//         if (dialogAsset != null)
//         {
//             previusId = dialogAsset.strings[index].id;
//         }

//         PrintSpeakers();
//     }

//     //ATTIVA IL CANCAS DELLO SPEAKER E IMPOSTA IL COLORE DEL FRAME
//     private void ActivateCanvas(GameObject gameObject, Color colorFrame) 
//     {
//         DialogBox canvas = gameObject.GetComponentInChildren<DialogBox>();

//         canvas.canvas.SetActive(true);

//         Image sp = canvas.gameObject.GetComponentInChildren<Image>();

//         sp.color = colorFrame;

//         if (gameObject.GetComponent<PlayerScript>())
//         {
//             if (dialogAsset.strings[index].bubblePosition == BubblePosition.Right)
//             {
//                 canvas.canvas.transform.position =
//                     gameObject.GetComponent<PlayerScript>().rightBubblePos.transform.position;
//             }
//             else
//             {
//                 canvas.canvas.transform.position =
//                     gameObject.GetComponent<PlayerScript>().leftBubblePos.transform.position;
//             }
//         }
//     }

//     //DISATTIVA IL CANVAS DELLO SPEAKER
//     private void DeactivateCanvas(GameObject gameObject) 
//     {
//         DialogBox canvas = gameObject.GetComponentInChildren<DialogBox>();

//         canvas.canvas.SetActive(false);
//     }

//     //CERCA IL TextMeshObj DELLO SPEAKER E RESETTA IL TESTO
//     private void SetTextMeshObj(Color colorText)
//     {
//         dialogTextObj =
//             sortedSpeakerList[dialogAsset.strings[index].id].gameObject.GetComponentInChildren<TextMeshProUGUI>();

//         dialogTextObj.text = "";
//         dialogTextObj.color = colorText;
//     }

//     //SCEGLIE IL CANVAS DA DISATTIVARE E QUELLO DA ATTIVARE 
//     private void SetCanvasToActivate()
//     {
//         if (previusId != dialogAsset.strings[index].id)
//         {
//             DeactivateCanvas(sortedSpeakerList[previusId]);
//             previusId = dialogAsset.strings[index].id;
//         }

//         ActivateCanvas(sortedSpeakerList[dialogAsset.strings[index].id], dialogAsset.strings[index].colorFrame);
//     }

//     //CONTROLLA SE IL DIALOGO E' FINITO, ALTRIMENTI AUMENTA INDEX
//     private void EndOfDialogCheck()
//     {
//         if (index < dialogAsset.strings.Count - 1)
//         {
//             index++;
//             dialogStatus = DialagoStatus.EndOfSentence;
//         }
//         else
//         {
//             dialogStatus = DialagoStatus.EndOfDialog;            
//         }
//     }

//     private void SwitchControlAtEnd()
//     {
//         if (sortedSpeakerList.ContainsKey(1))
//         {
//             sortedSpeakerList[1].gameObject.GetComponent<ChooseControllerScript>().EndOfDialogChanges();

//             sortedSpeakerList[1].gameObject.GetComponent<PlayerInput>().
//                 SwitchCurrentActionMap(sortedSpeakerList[1].gameObject.GetComponent<DialogScript>().actionMapNameSwitch);
//             return;
//         }

//         //foreach (KeyValuePair<int, GameObject> entry in sortedSpeakerList)
//         //{
//         //    if (entry.Key == 1)
//         //    {
//         //        sortedSpeakerList[1].gameObject.GetComponent<ChooseControllerScript>().EndOfDialogChanges();
//         //        sortedSpeakerList[1].gameObject.GetComponent<PlayerInput>().
//         //        SwitchCurrentActionMap(sortedSpeakerList[1].gameObject.GetComponent<DialogScript>().actionMapNameSwitch);
//         //        return;
//         //    }
//         //}

//         sortedSpeakerList[0].gameObject.GetComponent<PlayerInput>().
//                 SwitchCurrentActionMap(sortedSpeakerList[0].gameObject.GetComponent<DialogScript>().actionMapNameSwitch);
//     }

//     public void SetPosition(GameObject setPoint)
//     {
//         //Debug.Log("SETPOSITION");
//         Transform playerTransform = GameObject.FindObjectOfType<PlayerScript>().gameObject.GetComponent<Transform>();
//         //Debug.Log(playerTransform);
//         playerTransform.position = new Vector2(setPoint.transform.position.x, playerTransform.position.y);
//     }

//     public void PrintSpeakers()
//     {
//         foreach (KeyValuePair<int, GameObject> entry in speakerList)
//         {
//             //Debug.Log("normal list - " + entry);
//         }

//         foreach (KeyValuePair<int, GameObject> entry in sortedSpeakerList)
//         {
//             //Debug.Log("ordered list" + entry);
//         }
//     }

//     private static void ChangeScene()
//     {
//         if (PlayerScript.Instance.eventObj != null
//                     && PlayerScript.Instance.eventObj.isChangeSceneDoor
//                     && GameController.Instance.canExit)
//         {
//             GameController.Instance.ChangeScene(PlayerScript.Instance.eventObj.newSceneName);
//         }
//     }

//     private static void SwitchToMinigameCam()
//     {
//         if (PlayerScript.Instance.eventObj != null 
//             && PlayerScript.Instance.eventObj.isMiniGame)
//         {
//             PlayerScript.Instance.eventObj.SwitchCam();
//         }
//     }

//     private void ActiveAllEvents()
//     {
//         if (PlayerScript.Instance.eventObj != null
//             && PlayerScript.Instance.eventObj.isActiveAllEvents)
//         {
//             EventController.Instance.ActivateAll();
//         }
//     }
// }