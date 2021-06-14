using System.Diagnostics;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameOfLife : MonoBehaviour
{
    [Range(0, 100)] public int stepsPerSecond = 1;
    public TextMeshProUGUI generationText;
    public Cell cellPrefab;
    private readonly int _boxWidth = Screen.width / 16;
    private readonly int _boxHeight = Screen.height / 16;

    private readonly Stopwatch _stopwatch = new Stopwatch();
    private Cell[,] _grid;
    private Transform _cellHolder;
    private int _generation;

    private void Start()
    {
        SetupGame();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) return;
        if (stepsPerSecond == 0)
        {
            if (!Input.GetKeyDown(KeyCode.Return)) return;
            IncreaseGeneration();
            return;
        }

        var limit = 1 / (float) stepsPerSecond * 1000;
        if (_stopwatch.ElapsedMilliseconds <= limit) return;
        IncreaseGeneration();
        _stopwatch.Restart();
    }

    private void SetupGame()
    {
        InitCamera();
        InitCells();

        _stopwatch.Start();
    }

    private void InitCamera()
    {
        var cam = FindObjectOfType<Camera>();
        var camX = (_boxWidth - 1) / 2;
        var camY = (_boxHeight - 1) / 2;
        cam.transform.position = new Vector3(camX + .5f, camY + .5f, -10);
        cam.orthographicSize = (float) _boxHeight / 2;
    }

    private void InitCells()
    {
        _cellHolder = new GameObject("Cell Holder").transform;
        _grid = new Cell[_boxWidth, _boxHeight];
        for (var x = 0; x < _boxWidth; x++)
        {
            for (var y = 0; y < _boxHeight; y++)
            {
                var cell = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity, _cellHolder);
                cell.name = $"[{x},{y}]";
                _grid[x, y] = cell;
                _grid[x, y].SetPosition(x, y);
                _grid[x, y].SetAlive(Random.Range(0, 100) > 75);
            }
        }
    }

    private void IncreaseGeneration()
    {
        _generation++;
        generationText.text = $"Generation: {_generation}";
// RULES:
// 1. Live cell with 2 or 3 live neighbours lives on.
// 2. All other live cells die.
// 3. Dead cell with 3 live neighbours becomes alive

        for (var x = 0; x < _boxWidth; x++)
        {
            for (var y = 0; y < _boxHeight; y++)
            {
                var neighbours = GetLiveNeighbours(x, y);

                if (_grid[x, y].IsAlive)
                {
                    if (neighbours != 2 && neighbours != 3)
                        _grid[x, y].SetAlive(false);
                }
                else
                {
                    if (neighbours == 3)
                        _grid[x, y].SetAlive(true);
                }
            }
        }
    }

    private int GetLiveNeighbours(int x, int y)
    {
        var neighbours = 0;
        // North
        if (y + 1 < _boxHeight)
        {
            if (_grid[x, y + 1].IsAlive)
                neighbours++;
        }

        // North East
        if (y + 1 < _boxHeight && x + 1 < _boxWidth)
        {
            if (_grid[x + 1, y + 1].IsAlive)
                neighbours++;
        }

        // East
        if (x + 1 < _boxWidth)
        {
            if (_grid[x + 1, y].IsAlive)
                neighbours++;
        }

        // South East
        if (x + 1 < _boxWidth && y - 1 >= 0)
        {
            if (_grid[x + 1, y - 1].IsAlive)
                neighbours++;
        }

        // South
        if (y - 1 >= 0)
        {
            if (_grid[x, y - 1].IsAlive)
                neighbours++;
        }

        // South West
        if (x - 1 >= 0 && y - 1 >= 0)
        {
            if (_grid[x - 1, y - 1].IsAlive)
                neighbours++;
        }

        // West
        if (x - 1 >= 0)
        {
            if (_grid[x - 1, y].IsAlive)
                neighbours++;
        }

        // North West
        if (y + 1 < _boxHeight && x - 1 >= 0)
        {
            if (_grid[x - 1, y + 1].IsAlive)
                neighbours++;
        }

        return neighbours;
    }

    [Button]
    private void ClearBoard()
    {
        foreach (var cell in _grid)
        {
            cell.SetAlive(false);
        }
    }
}