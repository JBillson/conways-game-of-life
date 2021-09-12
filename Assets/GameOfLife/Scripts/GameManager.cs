using System.Diagnostics;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameOfLife.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region PROPERTIES

        public Cell[,] Grid { get; set; }

        #endregion

        #region PUBLIC VARIABLES

        [Range(0, 100)] public int stepsPerSecond = 1;
        public TextMeshProUGUI generationText;
        public Cell cellPrefab;

        #endregion

        #region PRIVATE VARIABLES

        private readonly int _boardWidth = Screen.width / 16;
        private readonly int _boardHeight = Screen.height / 16;

        private Transform _cellHolder;
        private int _generation;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private bool _isPlaying;

        #endregion

        #region MONOBEHAVIOUR

        private void Start()
        {
            SetupGame();
        }

        private void Update()
        {
            // Increase Generation x times per second
            if (_stopwatch.ElapsedMilliseconds <= 1 / (float) stepsPerSecond * 1000 || !_isPlaying) return;
            IncreaseGeneration();
            _stopwatch.Restart();
        }

        #endregion

        #region PUBLIC METHODS

        public void RandomFill()
        {
            foreach (var cell in Grid)
                cell.SetAlive(Random.Range(0, 100) > 75);
        }

        public void IncreaseGeneration()
        {
            _generation++;
            generationText.text = $"Generation: {_generation}";
// RULES:
// 1. Live cell with 2 or 3 live neighbours lives on.
// 2. All other live cells die.
// 3. Dead cell with 3 live neighbours becomes alive

            for (var x = 0; x < _boardWidth; x++)
            {
                for (var y = 0; y < _boardHeight; y++)
                {
                    var neighbours = GetLiveNeighbours(x, y);

                    if (Grid[x, y].IsAlive)
                    {
                        if (neighbours != 2 && neighbours != 3)
                            Grid[x, y].SetAlive(false);
                    }
                    else
                    {
                        if (neighbours == 3)
                            Grid[x, y].SetAlive(true);
                    }
                }
            }
        }

        public void ClearBoard()
        {
            foreach (var cell in Grid)
                cell.SetAlive(false);
        }

        public void Play(int speed)
        {
            _isPlaying = true;
            stepsPerSecond = speed;
        }

        public void Stop()
        {
            _isPlaying = false;
            stepsPerSecond = 0;
        }

        #endregion

        #region PRIVATE METHODS

        private void SetupGame()
        {
            InitCamera();
            InitBoard();

            _stopwatch.Start();
        }

        private void InitCamera()
        {
            var cam = FindObjectOfType<Camera>();
            var camX = (_boardWidth - 1) / 2;
            var camY = (_boardHeight - 1) / 2;
            cam.transform.position = new Vector3(camX + .5f, camY + .5f, -10);
            cam.orthographicSize = (float) _boardHeight / 2;
        }

        private void InitBoard()
        {
            _cellHolder = new GameObject("Cell Holder").transform;
            Grid = new Cell[_boardWidth, _boardHeight];
            for (var x = 0; x < _boardWidth; x++)
            {
                for (var y = 0; y < _boardHeight; y++)
                {
                    var cell = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity, _cellHolder);
                    cell.name = $"[{x},{y}]";
                    Grid[x, y] = cell;
                    cell.SetAlive(false);
                }
            }
        }

        private int GetLiveNeighbours(int x, int y)
        {
            var neighbours = 0;
            // North
            if (y + 1 < _boardHeight)
            {
                if (Grid[x, y + 1].IsAlive)
                    neighbours++;
            }

            // North East
            if (y + 1 < _boardHeight && x + 1 < _boardWidth)
            {
                if (Grid[x + 1, y + 1].IsAlive)
                    neighbours++;
            }

            // East
            if (x + 1 < _boardWidth)
            {
                if (Grid[x + 1, y].IsAlive)
                    neighbours++;
            }

            // South East
            if (x + 1 < _boardWidth && y - 1 >= 0)
            {
                if (Grid[x + 1, y - 1].IsAlive)
                    neighbours++;
            }

            // South
            if (y - 1 >= 0)
            {
                if (Grid[x, y - 1].IsAlive)
                    neighbours++;
            }

            // South West
            if (x - 1 >= 0 && y - 1 >= 0)
            {
                if (Grid[x - 1, y - 1].IsAlive)
                    neighbours++;
            }

            // West
            if (x - 1 >= 0)
            {
                if (Grid[x - 1, y].IsAlive)
                    neighbours++;
            }

            // North West
            if (y + 1 < _boardHeight && x - 1 >= 0)
            {
                if (Grid[x - 1, y + 1].IsAlive)
                    neighbours++;
            }

            return neighbours;
        }

        #endregion
    }
}