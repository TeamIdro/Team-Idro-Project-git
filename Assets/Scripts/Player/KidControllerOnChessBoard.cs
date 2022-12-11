using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActionRecorder))]
public class KidControllerOnChessBoard : MonoBehaviour
{
    [SerializeField] private ActionRecorder actionRecorder;
    [SerializeField] private int m_maxTimeAvailable;
    [SerializeField, ReadOnly] private int m_currentTimeAvailable;
    public int CurrentTimeAvailable
    {
        get { return m_currentTimeAvailable; }
        set { m_currentTimeAvailable = value; }
    }
    public int MaxTimeAvailable
    {
        get { return m_maxTimeAvailable; }
    }

    public static GameObject currentRoomToEnter;

    private GamePlayInputActions m_actions;
    private RoomScript[] m_tilesThatThePlayerCanGo;

    private const float RAYCASTDISTANCEFROMPLAYER = 5f;

    private void Awake()
    {
        m_actions = new GamePlayInputActions();
    }
    public void Start()
    {
        m_currentTimeAvailable = m_maxTimeAvailable;
    }



    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        m_tilesThatThePlayerCanGo = CheckForTilesAvailable();
        if (m_tilesThatThePlayerCanGo != null && m_currentTimeAvailable > 0)
        {
            GetSelectedTile(m_tilesThatThePlayerCanGo);
        }
    }



    private RoomScript[] CheckForTilesAvailable()
    {
        List<RoomScript> roomScripts = new List<RoomScript>();
        RaycastHit2D[] hits;
        hits = Physics2D.BoxCastAll(transform.position, new Vector2(RAYCASTDISTANCEFROMPLAYER, RAYCASTDISTANCEFROMPLAYER), 0, Vector2.right, 1f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.GetComponent<RoomScript>())
            {
                roomScripts.Add(hits[i].collider.gameObject.GetComponent<RoomScript>());
                Debug.Log(roomScripts[i]);

            }
        }
        return roomScripts.ToArray();
    }
    private void GetSelectedTile(RoomScript[] m_tilesThatThePlayerCanGo)
    {
        if(m_tilesThatThePlayerCanGo == null) return;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(m_actions.KidMovement.MousePosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        for (int i = 0; i < m_tilesThatThePlayerCanGo.Length; i++)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == m_tilesThatThePlayerCanGo[i].gameObject)
                {
                    if (m_actions.KidMovement.MouseClick.WasPressedThisFrame())
                    {
                        GoToNextTile(m_tilesThatThePlayerCanGo[i].gameObject.transform.position);
                        m_tilesThatThePlayerCanGo[i].OnMouseClick.Invoke();
                        currentRoomToEnter = m_tilesThatThePlayerCanGo[i].gameObject;
                    }
                   

                }
            }
        }
    }

  

    public void GoToNextTile(Vector2 direction)
    {
        var movementAction = new MovementAction(this,direction);
        actionRecorder.Record(movementAction);
    }
    /// <summary>
    /// For Event Calling Only
    /// </summary>
    public void EnterRoomFromPlayer()
    {
        currentRoomToEnter.GetComponent<RoomScript>().EnterRoom();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(RAYCASTDISTANCEFROMPLAYER, RAYCASTDISTANCEFROMPLAYER));
    }







    private void OnEnable() =>m_actions.Enable();
    private void OnDisable() => m_actions.Disable();


}
