using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;


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
        MoveTo(cell);
    }


    /// <summary>
    /// 셀 위치 설정 및 이동
    /// </summary>
    /// <param name="cell"></param>
    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(m_CellPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x--;
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
                //옮기려는 곳으로 위치 설정 및 플레이어의 위치 변경
                MoveTo(newCellTarget);

                if(cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEntered();
                }

            }
            
        }

    }


}
