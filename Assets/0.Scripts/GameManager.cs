using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance { get; private set; }
    public BoardManager BoardManager;
    public PlayerController PlayerController;

    //property
    public TurnManager TurnManager{get; private set;}

    private int m_FoodAmount = 100;

    private void Awake()
    {
        if (Instance != null) //null인지 체크후 이미 존재한다면 하나만 남게 처리함.
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen; //OnTick이벤트에 메서드 등록하는 방법.
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }

    void OnTurnHappen()
    {
        m_FoodAmount --;
        Debug.Log($"Current Amount of Food : {m_FoodAmount}");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
