using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameOfLife.Scripts
{
    public class Cell : MonoBehaviour
    {
        public bool IsAlive { get; private set; }
        private SpriteRenderer _spriteRenderer;
        private EventTrigger _eventTrigger;
        private bool _canDrag;
        private Camera _camera;
        private bool _isAdding;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _eventTrigger = GetComponentInChildren<EventTrigger>();
            _camera = FindObjectOfType<Camera>();
        }

        private void Start()
        {
            var onPointerEnter = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter
            };
            onPointerEnter.callback.AddListener(x => OnPointerEnter());

            _eventTrigger.triggers.Add(onPointerEnter);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                LeftClick();

            _canDrag = Input.GetMouseButton(0);
        }

        private void LeftClick()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            if (hit.transform.parent.gameObject != gameObject) return;
            SetAlive(!IsAlive);
        }

        private void OnPointerEnter()
        {
            if (!_canDrag) return;
            SetAlive(!IsAlive);
        }

        public void SetAlive(bool alive)
        {
            StartCoroutine(Alive(alive));
        }

        private IEnumerator Alive(bool alive)
        {
            yield return new WaitForEndOfFrame();
            IsAlive = alive;
            _spriteRenderer.color = IsAlive ? Color.white : Color.blue;
        }
    }
}