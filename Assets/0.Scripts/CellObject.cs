using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int m_Cell;

    public virtual void Init(Vector2Int cell)
    {
        m_Cell = cell;
    }
    
    /// <summary>
    /// 플레이어가 타일에 들어갔을 때 발생하는 코드
    /// </summary>
    public virtual void PlayerEntered()
    {
        
    }

    public virtual bool PlayerWantsToEnter()
    {
        return true;
    }
}
