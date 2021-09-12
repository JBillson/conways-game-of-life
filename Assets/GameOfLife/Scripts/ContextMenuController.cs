using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Eden.EdenUnityUtilities.Utilities;
using Newtonsoft.Json;
using SFB;
using UnityEngine;

namespace GameOfLife.Scripts
{
    public class ContextMenuController : MonoBehaviour
    {
        private ContextMenuView _contextMenuView;
        private GameManager _gameManager;
        private int _speed;

        private void Awake()
        {
            _contextMenuView = FindObjectOfType<ContextMenuView>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        public void Play()
        {
            _speed = (int) _contextMenuView.speedSlider.value;
            _gameManager.Play(_speed);
        }

        public void Stop()
        {
            _gameManager.Stop();
        }

        public void Step()
        {
            _gameManager.IncreaseGeneration();
        }

        public void Clear()
        {
            _gameManager.ClearBoard();
        }

        public void RandomFill()
        {
            _gameManager.stepsPerSecond = 0;
            _gameManager.ClearBoard();
            _gameManager.RandomFill();
        }

        public void UpdateSpeed(int generationsPerSecond)
        {
            _gameManager.stepsPerSecond = generationsPerSecond;
        }

        public void Load()
        {
            var path = StandaloneFileBrowser.OpenFilePanel("Select your Grid File", "", "", false)[0];
        }

        public void Save()
        {
            var path = StandaloneFileBrowser.SaveFilePanel("Save your Grid File", "", $"Grid_{DateTime.UtcNow:yyyyMM}",
                "json");

            var grids = new List<bool[,]>();
            var gridJson = (from Cell cell in _gameManager.Grid select cell.IsAlive).ToList().ToJson(
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None
                });

            File.WriteAllText(path, gridJson);
            Debug.Log($"Saving grid to ({path})");
        }
    }
}