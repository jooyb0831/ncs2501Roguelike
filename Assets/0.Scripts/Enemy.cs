using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : CellObject
{
    public int Health = 5;
    private int m_CurrentHealth;

    [SerializeField] AudioClip clip;
    private void Awake()
    {
        GameManager.Instance.TurnManager.OnTick += TurnHappened;
    }

    private void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        m_CurrentHealth = Health;
    }

    public override bool PlayerWantsToEnter()
    {
        m_CurrentHealth--;

        if (m_CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
        return false;
    }

    bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null || !targetCell.Passable || targetCell.ContainedObject != null)
        {
            return false;
        }

        //적들을 현재 셀에서 삭제
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        //적들을 다음 셀에 추가
        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    void TurnHappened()
    {
        //플레이어가 있는 셀 받아오기
        var PlayerCell = GameManager.Instance.PlayerController.Cell;

        int xDist = PlayerCell.x - m_Cell.x;
        int yDist = PlayerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1)) // 옆에 있으면
        {
            //플레이어 공격
            GameManager.Instance.ChangeFood(-3);
            GameManager.Instance.PlaySound(clip);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist)) //Try해서 안되면 다른방향으로
                {
                    //공격받지 않고 이동하지 않으면 Y좌표를 따라 움직이게 설정
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
        }
    }

    bool TryMoveInX(int xDist)
    {
        if (xDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.right);
        }

        return MoveTo(m_Cell + Vector2Int.left);
    }

    bool TryMoveInY(int yDist)
    {
        if (yDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.up);
        }
        return MoveTo(m_Cell + Vector2Int.down);
    }

}
