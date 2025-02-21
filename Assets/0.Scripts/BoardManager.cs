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

    //타일맵 컴포넌트
    private Tilemap m_Tilemap;

    //생성할 타일맵의 크기
    public int Width;
    public int Height;

    //그릴 타일을 담을 배열
    public Tile[] GroundTiles; //바닥
    public Tile[] WallTiles; //벽

    // Start is called before the first frame update
    void Start()
    {
        //타일맵 컴포넌트 받아오기
        m_Tilemap = GetComponentInChildren<Tilemap>();

        //보드 데이터 설정
        m_BoardData = new CellData[Width, Height];

        //높이만큼 돌림
        for (int y = 0; y < Height; y++)
        {
            //너비만큼 돌림
            for (int x = 0; x < Width; x++)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();
                //가장자리일 경우
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    //벽타일 랜덤으로 가져옴
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
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

    // Update is called once per frame
    void Update()
    {

    }
}
