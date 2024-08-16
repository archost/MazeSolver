using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver
{
    private int INF = 1000000000;

    public IEnumerator BFS(List<List<GameObject>> maze, int[,] map, KeyValuePair<int, int> start, KeyValuePair<int, int> finish)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        Func<int, int, bool> isInside = (row, col) => 0 <= row && row < rows && 0 <= col && col < cols;

        int[,] dist = new int[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                dist[i, j] = INF;

        KeyValuePair<int, int>[,] from = new KeyValuePair<int, int>[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                from[i, j] = new(-1, -1);

        dist[start.Key,start.Value] = 0;

        Queue<KeyValuePair<int, int>> q = new();
        q.Enqueue(new KeyValuePair<int, int>(start.Key, start.Value));

        KeyValuePair<int, int>[] dir = { new(0, 1), new(1, 0), new (0, -1), new(-1, 0) };

        while(q.Count > 0)
        {
            var cell = q.Dequeue();
            foreach(var pp in dir)
            {
                int ty = cell.Key + pp.Key;
                int tx = cell.Value + pp.Value;

                if (isInside(ty, tx) && map[ty, tx] != 1 && dist[ty, tx] > dist[cell.Key, cell.Value] + 1)
                {
                    dist[ty, tx] = dist[cell.Key, cell.Value] + 1;
                    from[ty, tx] = new(cell.Key, cell.Value);
                    maze[ty][tx].GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                    q.Enqueue(new(ty, tx));
                    // yield return new WaitForSeconds(0.05f);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        if (dist[finish.Key, finish.Value] != INF)
        {
            int y = finish.Key;
            int x = finish.Value;
            while(y != -1 && x != -1)
            {
                maze[y][x].GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
                int prevY = y;
                int prevX = x;
                y = from[prevY, prevX].Key;
                x = from[prevY, prevX].Value;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
