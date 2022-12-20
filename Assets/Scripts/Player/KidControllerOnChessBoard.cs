using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActionRecorder))]
[DisallowMultipleComponent]
public class KidControllerOnChessBoard : MonoBehaviour,IDialogueSpeaker
{
    [SerializeField] private ActionRecorder actionRecorder;
    [SerializeField] private int m_maxTimeAvailable;
    [SerializeField, ReadOnly] private int m_currentTimeAvailable;
    [SerializeField] private LayerMask m_tilesMask;
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
    private MovementAction movementAction;

    private const float RAYCASTDISTANCEFROMPLAYER = 4.75f;

    private void Awake()
    {
        m_actions = new GamePlayInputActions();
    }
    public void Start()
    {
        m_currentTimeAvailable = m_maxTimeAvailable;
        GetCurrentPlayerPosition();
    }



    private void Update()
    {
        Movement();
        GetCurrentPlayerPosition();
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
        hits = Physics2D.BoxCastAll(transform.position, new Vector2(RAYCASTDISTANCEFROMPLAYER, RAYCASTDISTANCEFROMPLAYER), 0, Vector2.zero, 1f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.GetComponent<RoomScript>())
            {
                roomScripts.Add(hits[i].collider.gameObject.GetComponent<RoomScript>());
            }
        }
        return roomScripts.ToArray();
    }
    private void GetSelectedTile(RoomScript[] m_tilesThatThePlayerCanGo)
    {
        if(m_tilesThatThePlayerCanGo == null) return;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(m_actions.Kid.MousePosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        for (int i = 0; i < m_tilesThatThePlayerCanGo.Length; i++)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == m_tilesThatThePlayerCanGo[i].gameObject)
                {
                    if (m_actions.Kid.MouseClick.WasPressedThisFrame())
                    {
                        GoToNextTile(m_tilesThatThePlayerCanGo[i].gameObject.transform.position);
                        m_tilesThatThePlayerCanGo[i].OnMouseClick.Invoke();
                    }
                   

                }
            }
        }
    }
    public void GetCurrentPlayerPosition()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector3.forward, 0.1f, m_tilesMask);
        Debug.Log(hit.collider);
        if (hit.collider != null)
        {
            currentRoomToEnter = hit.collider.gameObject;
        }
    }
    public void GoToNextTile(Vector2 direction)
    {
        movementAction = new MovementAction(this,direction,transform.position);
        actionRecorder.Record(movementAction);
    }
    /// <summary>
    /// For Event Calling Only
    /// </summary>
    public void EnterRoomFromPlayer()
    {
        currentRoomToEnter.GetComponent<RoomScript>().EnterRoom();
    }
    public void Speak()
    {
        //Codice per chiamare il Dialogue System
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(RAYCASTDISTANCEFROMPLAYER, RAYCASTDISTANCEFROMPLAYER));
    }







    private void OnEnable() =>m_actions.Enable();
    private void OnDisable() => m_actions.Disable();

    
}
