using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public Tile HP1Tile;
    [SerializeField] Sprite[] TileSprites;
    [SerializeField] Tile tempTile;
    public const int MAX_HEALTH = 3;

    private int m_HealthPoint;
    private Tile m_OriginalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HealthPoint = MAX_HEALTH;
        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile);

    }

    public override bool PlayerWantsToEnter()
    {
        m_HealthPoint --;

        if(m_HealthPoint > 0)
        {
            if (m_HealthPoint == 1)
            {
                
                GameManager.Instance.BoardManager.SetCellTile(m_Cell, HP1Tile);
            }
            return false;
        }
        GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
        
        Destroy(gameObject);
        return true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


}
