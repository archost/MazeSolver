using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    [SerializeField]
    private GameObject _cellPrefab;

    private float _screenHeight, _screenWidth;

    public float prefabSize;

    void Start()
    {
        Camera camera = Camera.main;
        _screenHeight = camera.orthographicSize * 2;
        _screenWidth = camera.orthographicSize * 2 * camera.aspect;

        Vector2 cameraPosition = camera.transform.position;
        transform.position = new Vector2(cameraPosition.x - _screenWidth / 2, cameraPosition.y - _screenHeight / 2);
    }

    public List<List<GameObject>> DrawMaze(int[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        List<List<GameObject>> maze = new List<List<GameObject>>(rows);

        float prefabHeight, prefabWidth;
        prefabHeight = _screenHeight / rows;
        prefabWidth = _screenWidth / cols;

        prefabSize = Math.Min(prefabWidth, prefabHeight);

        for (int y = 0, mazeY = rows - 1; y < rows; y++, mazeY--)
        {
            List<GameObject> row = new List<GameObject>(cols);
            for (int x = 0; x < cols; x++)
            {
                GameObject instance = Instantiate(_cellPrefab);
                Color color = Color.white;

                switch(map[y, x])
                {
                    case (1):
                        color = Color.white;
                        break;
                    case (-1):
                        color = Color.black;
                        break;
                }

                instance.GetComponentInChildren<SpriteRenderer>().color = color;
                instance.transform.localScale = new Vector2(prefabSize, prefabSize);
                instance.transform.position = new Vector2(transform.position.x + prefabSize * x, transform.position.y + prefabSize * mazeY);
                row.Add(instance);
            }
            maze.Add(row);
        }

        return maze;
    }
}