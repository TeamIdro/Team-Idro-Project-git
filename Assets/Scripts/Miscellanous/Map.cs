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


    private KidControllerOnChessBoard m_playerReference;
    private List<GameObject> m_firstRoomList = new List<GameObject>();

    private void Awake()
    {
        //if (GameManager.Instance.playerPrefab_ChessVersion != null)
        //{
        //    m_playerReference = GameManager.Instance.playerPrefab_ChessVersion.GetComponent<KidControllerOnChessBoard>();
        //}
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
        m_playerReference.transform.position = Vector3.MoveTowards(m_playerReference.transform.position, firstRoom.transform.position,Time.deltaTime* 5f);
        yield return new WaitUntil(() => m_playerReference.transform.position == firstRoom.transform.position);
    }
}
