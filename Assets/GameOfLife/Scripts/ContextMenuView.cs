using Lean.Gui;
using UnityEngine;

namespace GameOfLife.Scripts
{
    public class ContextMenuView : MonoBehaviour
    {
        [Header("Main Menu")] public LeanWindow mainMenuWindow;
        public LeanButton createMenuButton;
        public LeanButton clearBoardButton;

        [Header("Create Menu")] public LeanWindow createMenuWindow;
        public LeanButton gosperGliderGunButton;
        public LeanButton gosperGliderEater;

        [Header("Other")] public LeanButton hideContextMenuButton;
        public LeanButton backButton;

        private ContextMenuController _contextMenuController;
        private GameManager _gameManager;
        private ShapeCreator _shapeCreator;

        private void Awake()
        {
            _contextMenuController = FindObjectOfType<ContextMenuController>();
            _gameManager = FindObjectOfType<GameManager>();
            _shapeCreator = FindObjectOfType<ShapeCreator>();
        }

        private void Start()
        {
            // Main Menu
            createMenuButton.OnClick.AddListener(ShowCreateMenu);
            clearBoardButton.OnClick.AddListener(() => _gameManager.ClearBoard());

            // Create Menu
            gosperGliderGunButton.OnClick.AddListener(() =>
                _shapeCreator.CreateShape(ShapeCreator.Shape.GosperGliderGun));
            gosperGliderEater.OnClick.AddListener(() =>
                _shapeCreator.CreateShape(ShapeCreator.Shape.GosperGliderEater));

            // Other
            hideContextMenuButton.OnClick.AddListener(() => _contextMenuController.HideContextMenu());
            backButton.OnClick.AddListener(ShowMainMenu);

            ShowMainMenu();
        }

        private void ShowCreateMenu()
        {
            createMenuWindow.On = true;
        }

        private void ShowMainMenu()
        {
            mainMenuWindow.On = true;
        }
    }
}