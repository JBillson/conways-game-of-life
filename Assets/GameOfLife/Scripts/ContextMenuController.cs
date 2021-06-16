using System;
using UnityEngine;

namespace GameOfLife.Scripts
{
    public class ContextMenuController : MonoBehaviour
    {
        private ContextMenuView _contextMenuView;
        private Camera _camera;

        private void Awake()
        {
            _contextMenuView = FindObjectOfType<ContextMenuView>();
            _camera = FindObjectOfType<Camera>();
        }

        private void Start()
        {
            HideContextMenu();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                RightClick();
        }

        private void RightClick()
        {
            if (_contextMenuView.transform.parent.gameObject.activeInHierarchy) return;

            var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            ShowContextMenu(mousePos);
        }

        private void ShowContextMenu(Vector3 position)
        {
            var boardPosition = new Vector3(position.x, position.y, -5);
            _contextMenuView.transform.position = boardPosition;
            _contextMenuView.transform.parent.gameObject.SetActive(true);
        }

        public void HideContextMenu()
        {
            _contextMenuView.transform.parent.gameObject.SetActive(false);
            _contextMenuView.mainMenuWindow.On = true;
        }
    }
}