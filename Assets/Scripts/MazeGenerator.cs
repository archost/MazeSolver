using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int[,] GenerateMaze(int width, int height, out KeyValuePair<int, int> start, out KeyValuePair<int, int> finish)
    {
        int[,] maze = new int[height, width];

        // Initialize maze with walls
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                maze[i, j] = 1;
            }
        }

        start = new(Random.Range(1, height - 1), Random.Range(1, width - 1));
        finish = new(Random.Range(1, height - 1), Random.Range(1, width - 1));
        
        maze[start.Key, start.Value] = 2;
        maze[finish.Key, finish.Value] = 3;

        RecursiveBacktracker(maze, start.Key, start.Value);

        return maze;
    }

    private void RecursiveBacktracker(int[,] maze, int x, int y)
    {
        int[] directions = { 0, 1, 2, 3 };
        Shuffle(directions);

        foreach (int direction in directions)
        {
            int newX = x;
            int newY = y;

            switch (direction)
            {
                case 0: // Up
                    newY -= 2;
                    break;
                case 1: // Down
                    newY += 2;
                    break;
                case 2: // Left
                    newX -= 2;
                    break;
                case 3: // Right
                    newX += 2;
                    break;
            }

            if (newX > 0 && newX < maze.GetLength(1) - 1 && newY > 0 && newY < maze.GetLength(0) - 1 && maze[newY, newX] == 1)
            {
                maze[newY, newX] = 0;
                maze[y + (newY - y) / 2, x + (newX - x) / 2] = 0;
                RecursiveBacktracker(maze, newX, newY);
            }
        }
    }

    private void Shuffle(int[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + Random.Range(0, n - i);
            int temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
    }
}