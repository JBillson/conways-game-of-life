using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameOfLife.Scripts
{
    public class ContextMenuView : MonoBehaviour
    {
        [Header("Control Section")] public Button startButton;
        public Button stopButton;
        public Button clearButton;
        public Button randomFillButton;
        public Button stepButton;
        public Slider speedSlider;
        public TextMeshProUGUI speedText;

        [Header("Load/Save Section")] public Button loadButton;
        public Button saveButton;

        [Header("Other")] public Button hideContextMenuButton;

        private ContextMenuController _contextMenuController;

        private void Awake()
        {
            _contextMenuController = FindObjectOfType<ContextMenuController>();
        }

        private void Start()
        {
            startButton.onClick.AddListener(() => _contextMenuController.Play());
            stopButton.onClick.AddListener(_contextMenuController.Stop);
            clearButton.onClick.AddListener(_contextMenuController.Clear);
            randomFillButton.onClick.AddListener(_contextMenuController.RandomFill);
            stepButton.onClick.AddListener(_contextMenuController.Step);
            speedSlider.onValueChanged.AddListener(value =>
            {
                var speed = (int) value;
                speedText.text = speed.ToString();
                _contextMenuController.UpdateSpeed(speed);
            });


            saveButton.onClick.AddListener(_contextMenuController.Save);
            loadButton.onClick.AddListener(_contextMenuController.Load);
        }
    }
}