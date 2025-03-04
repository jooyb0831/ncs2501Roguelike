using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
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

    //셀 오브젝트
    public FoodObject[] FoodPrefab; //음식 프리팹
    public WallObject[] WallPrefab; //벽(장애물)타일
    public ExitCellObject ExitCellPrefab; //출구 타일

    public List<Vector2Int> m_EmptyCellsLists;

    [SerializeField] int number1;
    [SerializeField] int number2;


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

        //비어있는 셀 리스트 초기화
        m_EmptyCellsLists = new List<Vector2Int>();


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

                    //비어있고 이동가능한 셀을 리스트에 추가
                    m_EmptyCellsLists.Add(new Vector2Int(x, y));
                }

                //해당하는 x,y 값에 랜덤으로 뽑은 타일 그리기
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        //플레이어가 있는 위치의 셀은 emptycellList에서 빼기
        m_EmptyCellsLists.Remove(new Vector2Int(1, 1));

        //출구타일 위치 지정
        Vector2Int endCoord = new Vector2Int(Width - 2, Height - 2);

        //출구타일 배치
        AddObject(Instantiate(ExitCellPrefab), endCoord);

        //비어있는 셀 리스트에서 출구타일 지우기
        m_EmptyCellsLists.Remove(endCoord);

        //음식과 장애물 벽 생성
        GenerateWall();
        GenerateFood();
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

    /// <summary>
    /// 타일 세팅
    /// </summary>
    /// <param name="cellIndex"></param>
    /// <param name="tile"></param>
    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
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

    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }


    void GenerateFood()
    {
        int foodCount = Random.Range(number1, number2);
        for (int i = 0; i < foodCount; i++)
        {
            //비어있고 이동 가능한 셀의 인덱스 받기
            int randomIndex = Random.Range(1, m_EmptyCellsLists.Count);

            //인덱스로 해당 좌표 찾기
            Vector2Int coord = m_EmptyCellsLists[randomIndex];

            //emptyCell리스트에서 해당 셀의 인덱스 제거
            m_EmptyCellsLists.RemoveAt(randomIndex);
            //음식생성
            int randFoodIdx = Random.Range(0, FoodPrefab.Length);
            FoodObject newFood = Instantiate(FoodPrefab[randFoodIdx]);
            AddObject(newFood, coord);

            /*
            //좌표설정
            CellData data = m_BoardData[coord.x, coord.y];

            //음식생성
            int randFoodIdx = Random.Range(0, FoodPrefab.Length);
            FoodObject newFood = Instantiate(FoodPrefab[randFoodIdx]);

            //위치변경
            newFood.transform.position = CellToWorld(coord);

            data.ContainedObject = newFood;
            */
        }
    }


    /// <summary>
    /// 장애물 벽 생성
    /// </summary>
    void GenerateWall()
    {
        int wallCount = Random.Range(6, 10);
        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsLists.Count);
            Vector2Int coord = m_EmptyCellsLists[randomIndex];

            m_EmptyCellsLists.RemoveAt(randomIndex);

            int rand = Random.Range(0, WallPrefab.Length);
            WallObject newWall = Instantiate(WallPrefab[rand]);
            AddObject(newWall, coord);

            /*
            //init the wall
            newWall.Init(coord);

            newWall.transform.position = CellToWorld(coord);
            data.ContainedObject = newWall;
            */
        }
    }

    public void Clean()
    {
        if (m_BoardData == null) return;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var cellData = m_BoardData[x, y];

                if (cellData.ContainedObject != null)
                {
                    //Destroy(cellData.ContainedObject) 컴포넌트를 삭제함
                    //셀 삭제
                    Destroy(cellData.ContainedObject.gameObject);
                }
                //타일 모두 초기화(null)
                SetCellTile(new Vector2Int(x, y), null);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
