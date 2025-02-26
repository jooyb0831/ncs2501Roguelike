using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
    }

    //셀 데이터 받아오는 2차원 배열
    private CellData[,] m_BoardData;

    //플레이어
    public PlayerController Player;

    //타일맵 컴포넌트
    private Tilemap m_Tilemap;

    //그리드 컴포넌트
    private Grid m_Grid;

    //생성할 타일맵의 크기
    public int Width;
    public int Height;

    //그릴 타일을 담을 배열
    public Tile[] GroundTiles; //바닥
    public Tile[] BlockingTiles; //벽

    

    // Start is called before the first frame update
    void Start()
    {

        //플레이어 스폰
        //Player.Spawn(this, new Vector2Int(1, 1));
    }

    public void Init()
    {
        //타일맵 컴포넌트 받아오기
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponent<Grid>();

        //보드 데이터 설정 (전체 새로 지칭)
        m_BoardData = new CellData[Width, Height];

        //높이만큼 돌림
        for (int y = 0; y < Height; y++)
        {
            //너비만큼 돌림
            for (int x = 0; x < Width; x++)
            {
                Tile tile;

                //각각의 셀 데이터는 따로 만들어줘야 함
                m_BoardData[x, y] = new CellData();

                //가장자리일 경우
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    //벽타일 랜덤으로 가져옴
                    tile = BlockingTiles[Random.Range(0, BlockingTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }

                else
                {
                    //바닥 타일 랜덤으로 가져옴
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                }

                //해당하는 x,y 값에 랜덤으로 뽑은 타일 그리기
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }


    /// <summary>
    /// cell인덱스를 Grid의 World포지로 변경
    /// </summary>
    /// <param name="cellIndex"></param>
    /// <returns></returns>
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }


    public CellData GetCellData(Vector2Int cellIndex)
    {
        //범위체크(범위 안 벗어나게끔)
        if (cellIndex.x < 0 || cellIndex.x >= Width
        || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }

        //셀 인덱스 값 리턴
        return m_BoardData[cellIndex.x, cellIndex.y];
    }
    // Update is called once per frame
    void Update()
    {

    }
}
