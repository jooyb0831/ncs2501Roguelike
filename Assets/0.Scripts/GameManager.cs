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

    private VisualElement m_GameOverPanel;
    private Label m_FoodLabel;
    private Label m_LevelLabel;
    private Label m_GameOverMessage;

    private int m_CurrentLevel = 1;
    public int Level
    {
        get
        {
            return m_CurrentLevel;
        }
        private set{}
    }

    private int m_FoodAmount = 40;
    private const string GOS1 = "GAME OVER";
    private const string GOS2 = "You Traveled\n\nthrough";
    private const string GOS3 = "levels!";
    private const string GOS4 = "Press 'ENTER'\n\nto Restart.";
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
        m_LevelLabel = UIDoc.rootVisualElement.Q<Label>("LevelLabel");
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        m_FoodLabel.text = $"Food : {m_FoodAmount}";
        m_LevelLabel.text = $"Level : {m_CurrentLevel}";
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen; //OnTick이벤트에 메서드 등록하는 방법.
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }

    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));

        m_CurrentLevel ++;
        m_LevelLabel.text = $"Level : {m_CurrentLevel}";
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

        if(m_FoodAmount <= 0)
        {
            m_FoodAmount = 0;
            m_FoodLabel.text = $"Food : {m_FoodAmount:000}";
            PlayerController.GameOver();
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = $"{GOS1}\n\n<color=white>{GOS2}</color> <color=yellow>{m_CurrentLevel}</color> <color=white>{GOS3}\n\n\n{GOS4}</color>";
        }
    }

    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;

        m_CurrentLevel = 0;
        m_FoodAmount = 20;
        m_FoodLabel.text = $"Food : {m_FoodAmount}";
        m_LevelLabel.text = $"Level : {m_CurrentLevel}";

        NewLevel();
        PlayerController.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
