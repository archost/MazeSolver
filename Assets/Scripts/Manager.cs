using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    private MazeGenerator _mazeGenerator  = new();

    private MazeSolver _mazeSolver = new();

    [SerializeField]
    private MazeVisualizer _mazeVisualizer;

    private int _cols;

    private int _rows;

    [SerializeField]
    private InputActionAsset inputActions;

    private InputAction _generateAction;

    private InputAction _solveAction; 

    private InputAction _resetAction; 

    private List<List<GameObject>> _maze;

    private int[,] _map;

    private Coroutine currentCoroutine;

    private KeyValuePair<int, int> _start = new(-1, -1), _finish = new(-1, -1);

    private void Start()
    {
        _cols = MenuParameters.Instance.width;
        _rows = MenuParameters.Instance.height;

        var actionMap = inputActions.FindActionMap("Player");
        _generateAction = actionMap.FindAction("GenerateMap");
        _solveAction = actionMap.FindAction("Solve");
        _resetAction = actionMap.FindAction("Reset");

        _generateAction.performed += OnGenerateMap;
        _solveAction.performed += OnSolve;
        _resetAction.performed += OnResetMap;

        _generateAction.Enable();
        _solveAction.Enable();
        _resetAction.Enable();
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if (_maze != null)
                SetStartAndFinish(worldPosition);
        }
        
    }

    private void OnGenerateMap(InputAction.CallbackContext context)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        if (_maze != null)
            DeleteMaze();

        _start = new(-1, -1);
        _finish = new(-1, -1);

        _map = _mazeGenerator.GenerateMaze(_cols, _rows);
        _maze = _mazeVisualizer.DrawMaze(_map);
    }

    private void OnResetMap(InputAction.CallbackContext context)
    {
        if (_maze == null)
            return;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        DeleteMaze();

        _start = new(-1, -1);
        _finish = new(-1, -1);

        _maze = _mazeVisualizer.DrawMaze(_map);
    }

    private void OnSolve(InputAction.CallbackContext context)
    {
        if (_start.Key != -1 && _start.Value != -1 && _finish.Key != -1 && _finish.Value != -1
            && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(_mazeSolver.Astar(_maze, _map, _start, _finish));
        }
    }

    private void DeleteMaze()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                Destroy(_maze[i][j]);
            }
        }
    }
    public void SetStartAndFinish(Vector2 coordinates){
        if (_start.Key == -1 && _start.Value == -1)
        {
            _start = new((int)((-coordinates.y + 4) / _mazeVisualizer.prefabSize), (int)((coordinates.x + 7.11) / _mazeVisualizer.prefabSize));
            _maze[_start.Key][_start.Value].GetComponentInChildren<SpriteRenderer>().color = Color.green;
            // _map[_start.Key, _start.Value] = 1;
        }
        else if (_finish.Key == -1 && _finish.Value == -1)
        {
            _finish = new((int)((-coordinates.y + 4) / _mazeVisualizer.prefabSize), (int)((coordinates.x + 7.11) / _mazeVisualizer.prefabSize));
            _maze[_finish.Key][_finish.Value].GetComponentInChildren<SpriteRenderer>().color = Color.red;
            // _map[_finish.Key, _finish.Value] = 1;
        }
    }
}
