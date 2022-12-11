using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<GameObject> scacchiera;
    [Space]
    public GameObject firstRoom;
    [ReadOnly]
    public GameObject currentRoom;
    public KidControllerOnChessBoard playerReference;

    private List<GameObject> m_firstRoomList = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < scacchiera.Count; i++)
        {
            if (scacchiera[i].GetComponent<RoomScript>().IsFirstRoom)
            {
                m_firstRoomList.Add(scacchiera[i]);
                firstRoom = scacchiera[i];
            }
            else if (m_firstRoomList.Count >= 2)
            {
                Debug.LogError("solo un'oggetto può essere la prima stanza");
                return;
            }
        }

    }
    private void Start()
    {
        StartCoroutine(MoveToFirstRoom());
    }
    private void Update()
    {
        if (KidControllerOnChessBoard.currentRoomToEnter != null)
        {
            currentRoom = KidControllerOnChessBoard.currentRoomToEnter;
        }
    }
    private IEnumerator MoveToFirstRoom()
    {
        playerReference.transform.position = Vector3.MoveTowards(playerReference.transform.position, firstRoom.transform.position,Time.deltaTime* 5f);
        yield return new WaitUntil(() => playerReference.transform.position == firstRoom.transform.position);
    }
}
