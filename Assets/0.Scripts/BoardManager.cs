using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{   
    //타일맵 컴포넌트
    private Tilemap tilemap;

    //생성할 타일맵의 크기
    public int Width;
    public int Height;

    //그릴 타일을 담을 배열
    public Tile[] GroundTiles;

    // Start is called before the first frame update
    void Start()
    {
        //타일맵 컴포넌트 받아오기
        tilemap = GetComponentInChildren<Tilemap>();

        //높이만큼 돌림
        for (int y = 0; y < Height; ++y)
        {
            //너비만큼 돌림
            for (int x = 0; x < Width; ++x)
            {
                //타일맵 배열에서 랜덤으로 타일 가져옴
                int tileNum = Random.Range(0, GroundTiles.Length); 
                
                //해당하는 x,y 값에 랜덤으로 뽑은 타일 그리기
                tilemap.SetTile(new Vector3Int(x, y, 0), GroundTiles[tileNum]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
