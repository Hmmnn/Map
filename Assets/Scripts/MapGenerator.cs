using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Range(0, 100)]
    public int wallPercent;
    [SerializeField]
    private int iteration;

    private string randomSeed;

    int[,] map;
    int[,] compareCopy;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        map = new int[width, height];

        // 랜덤한 위치에 벽 설정
        SetWallAtRandomPos();

        // 설정한 벽 주변 타일 검사
        // iteration만큼 반복
        for (int i = 0; i < iteration; i++)
        {
            // 원본 맵과 비교용 맵 분리
            compareCopy = map;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    CheckSurroundings(x, y);
            }
        }
    }

    private void SetWallAtRandomPos()
    {
        randomSeed = System.DateTime.Now.ToString();

        System.Random random = new System.Random(randomSeed.GetHashCode());

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    // 1 : 벽 / 0 : 길
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (random.Next(0, 100) < wallPercent) ? 1 : 0;
                }
            }
        }
    }

    private void CheckSurroundings(int curX, int curY)
    {
        int wallCount = 0;

        for (int y = curY - 1; y <= curY + 1; y++)
        {
            for (int x = curX - 1; x <= curX + 1; x++)
            {
                if (x == curX && y == curY)
                    continue;

                if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
                    wallCount++;
                else if (compareCopy[x, y] == 1)
                    wallCount++;
            }
        }

        if (wallCount > 4)
            map[curX, curY] = 1;
        else if(wallCount < 4)
            map[curX, curY] = 0;
    }

    private void InitGrid()
    {

    }

    private void OnDrawGizmos()
    {
        if(map != null)
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, 0, -height / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
