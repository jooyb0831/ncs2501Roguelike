using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    public event System.Action OnTick;
    private int m_TurnCount;

    public TurnManager() //생성자
    {
        m_TurnCount = 1;
    }

    public void Tick()
    {
        m_TurnCount ++;
        Debug.Log($"Current Turn Count : {m_TurnCount}");

        //OnTick 이벤트에 등록된 모든 콜백 메서드를 호출하는 System.Action의 매서드.
        OnTick?.Invoke(); // ? : null이 아닌 경우에 해라

    }
}
