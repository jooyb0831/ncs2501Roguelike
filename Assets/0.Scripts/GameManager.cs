using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance { get; private set; }
    public BoardManager BoardManager;
    public PlayerController PlayerController;

    //property
    public TurnManager TurnManager{get; private set;}
    public UIDocument UIDoc;

    private Label m_FoodLabel;

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
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = $"Food : {m_FoodAmount}";

        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen; //OnTick이벤트에 메서드 등록하는 방법.
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
        Debug.Log($"Current Amount of Food : {m_FoodAmount}");
    }

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = $"Food : {m_FoodAmount:000}";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
