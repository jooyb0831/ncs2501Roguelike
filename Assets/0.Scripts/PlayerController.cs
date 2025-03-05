using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    private bool m_IsMoving;
    private Vector3 m_MoveTarget;

    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    private bool m_IsGameOver;

    private Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 플레이어를 보드에 스폰하는 코드
    /// </summary>
    /// <param name="boardManager"></param>
    /// <param name="cell"></param>
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        //변수 받아서 지정
        m_Board = boardManager;
        m_CellPosition = cell;

        //위치선정
        MoveTo(cell, true);
    }


    /// <summary>
    /// 셀 위치 설정 및 이동
    /// </summary>
    /// <param name="cell"></param>
    public void MoveTo(Vector2Int cell, bool immediate = false)
    {
        m_CellPosition = cell;
        if (immediate)
        {
            m_IsMoving = false;
            transform.position = m_Board.CellToWorld(m_CellPosition);
        }
        else
        {
            m_IsMoving = true;
            m_MoveTarget = m_Board.CellToWorld(m_CellPosition);
        }
        m_Animator.SetBool("Moving", m_IsMoving);
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        m_IsGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsGameOver)
        {
            if(Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }
        if(m_IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);

            if(transform.position == m_MoveTarget)
            {
                m_IsMoving = false;
                m_Animator.SetBool("Moving", false);
                var cellData = m_Board.GetCellData(m_CellPosition);
                if(cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEntered();
                    
                    m_Animator.SetTrigger("Attack");
                }
            }
            
            return;
        }
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        //기존의 InputManager가 아닌 새로운 InputSystem을 활용
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y++;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y--;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x++;
            transform.localScale = Vector3.one;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x--;
            transform.localScale = new Vector3(-1,1,1);
            hasMoved = true;
        }

        if (hasMoved)
        {
            //움직이려는 셀의 데이터 가져오기
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            //셀데이터가 Null이 아니고, 이동할 수 있는 곳이면 이동
            if (cellData != null && cellData.Passable)
            {   
                //턴수체크(GameManager싱글톤 통해 접근)
                GameManager.Instance.TurnManager.Tick();

                //이동하려는 타일이 오브젝트 포함하고 있지 않은 경우 바로 이동
                if(cellData.ContainedObject == null)
                {
                    //옮기려는 곳으로 위치 설정 및 플레이어의 위치 변경
                    MoveTo(newCellTarget);
                }

                else if (!cellData.ContainedObject.PlayerWantsToEnter())
                {
                    m_Animator.SetTrigger("Attack");
                    return;
                }
                
                //이동하려는 셀에 장애물이 있어 PlayerWnatsToEnter값이 True인 경우
                else if(cellData.ContainedObject.PlayerWantsToEnter())
                {
                    
                    MoveTo(newCellTarget);
                    //플레이어 먼저 이동하고 호출
                    //cellData.ContainedObject.PlayerEntered();
                }



            }
            
        }


    }
    
    public void GameOver()
    {
        m_IsGameOver = true;
    }

}
