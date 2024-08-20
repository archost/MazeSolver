using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

        while (q.Count > 0)
        {
            var cell = q.Dequeue();

            if (cell.Equals(finish))
                break;

            foreach(var pp in dir)
            {
                int ty = cell.Key + pp.Key;
                int tx = cell.Value + pp.Value;

                if (isInside(ty, tx) && map[ty, tx] != -1 && dist[ty, tx] > dist[cell.Key, cell.Value] + 1)
                {
                    dist[ty, tx] = dist[cell.Key, cell.Value] + 1;
                    from[ty, tx] = new(cell.Key, cell.Value);
                    maze[ty][tx].GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                    q.Enqueue(new(ty, tx));
                    yield return new WaitForSeconds(0.001f);
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

    public IEnumerator Dijkstra(List<List<GameObject>> maze, int[,] map, KeyValuePair<int, int> start, KeyValuePair<int, int> finish)
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

        dist[start.Key, start.Value] = 0;
        PriorityQueue<(int, int, int), int> pq = new PriorityQueue<(int, int, int), int>();
        pq.Enqueue((start.Key, start.Value, 0), 0);

        KeyValuePair<int, int>[] dir = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };

        while (pq.Count > 0)
        {
            var (x, y, distance) = pq.Dequeue();

            if (distance != dist[x, y])
                continue;

            if (x == finish.Key && y == finish.Value)
                break;

            foreach (var pp in dir)
            {
                int ty = x + pp.Key;
                int tx = y + pp.Value;
                int newDistance = map[x, y] + distance;

                if (isInside(ty, tx) && map[ty, tx] != -1 && dist[ty, tx] > newDistance)
                {
                    dist[ty, tx] = newDistance;
                    from[ty, tx] = new(x, y);
                    maze[ty][tx].GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                    pq.Enqueue((ty, tx, newDistance), newDistance);
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }

        if (dist[finish.Key, finish.Value] != INF)
        {
            int y = finish.Key;
            int x = finish.Value;
            while (y != -1 && x != -1)
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

    public IEnumerator Astar(List<List<GameObject>> maze, int[,] map, KeyValuePair<int, int> start, KeyValuePair<int, int> finish)
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

        dist[start.Key, start.Value] = 0;
        PriorityQueue<(int, int, int, int), float> pq = new PriorityQueue<(int, int, int, int), float>();
        pq.Enqueue((start.Key, start.Value, 0, 0), 0);

        KeyValuePair<int, int>[] dir = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };

        while (pq.Count > 0)
        {
            var (x, y, distance, f) = pq.Dequeue();

            if (distance != dist[x, y])
                continue;

            if (x == finish.Key && y == finish.Value)
                break;

            foreach (var pp in dir)
            {
                int ty = x + pp.Key;
                int tx = y + pp.Value;
                int newDistance = map[x, y] + distance;

                if (isInside(ty, tx) && map[ty, tx] != -1 && dist[ty, tx] > newDistance)
                {
                    dist[ty, tx] = newDistance;
                    from[ty, tx] = new(x, y);
                    maze[ty][tx].GetComponentInChildren<SpriteRenderer>().color = Color.blue;

                    float priority = newDistance + ManhattanHeuristic(ty, tx, finish.Key, finish.Value);

                    pq.Enqueue((ty, tx, newDistance, (int)priority), priority);
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }

        if (dist[finish.Key, finish.Value] != INF)
        {
            int y = finish.Key;
            int x = finish.Value;
            while (y != -1 && x != -1)
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

    private int ManhattanHeuristic(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    private int ChebyshevHeuristic(int x1, int y1, int x2, int y2)
    {
        return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
    }

    static float EuclideanHeuristic(int x1, int y1, int x2, int y2)
    {
        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }
}

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private SortedSet<(TElement element, TPriority priority)> _set;

    public PriorityQueue()
    {
        _set = new SortedSet<(TElement element, TPriority priority)>(Comparer<(TElement element, TPriority priority)>.Create((a, b) =>
        {
            int result = a.priority.CompareTo(b.priority);
            if (result == 0)
            {
                result = a.element.GetHashCode().CompareTo(b.element.GetHashCode());
            }
            return result;
        }));
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        _set.Add((element, priority));
    }

    public TElement Dequeue()
    {
        if (_set.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        var element = _set.Min;
        _set.Remove(element);
        return element.element;
    }

    public int Count => _set.Count;
}