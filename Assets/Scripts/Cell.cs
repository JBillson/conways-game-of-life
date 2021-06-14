using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public bool IsAlive { get; private set; }
    private SpriteRenderer _spriteRenderer;
    private EventTrigger _eventTrigger;
    private int _x;
    private int _y;
    private bool _canDrag;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _eventTrigger = GetComponentInChildren<EventTrigger>();
    }

    private void Start()
    {
        var onClickTrigger = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick,
        };
        onClickTrigger.callback.AddListener(x => OnClick());

        var onPointerEnter = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter
        };
        onPointerEnter.callback.AddListener(x => OnPointerEnter());

        _eventTrigger.triggers.Add(onClickTrigger);
        _eventTrigger.triggers.Add(onPointerEnter);
    }

    private void Update()
    {
        _canDrag = Input.GetMouseButton(0);
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

    public void SetPosition(int x, int y)
    {
        _x = x;
        _y = y;
    }

    private void OnClick()
    {
        SetAlive(!IsAlive);
    }

    private void OnPointerEnter()
    {
        if (!_canDrag) return;
        SetAlive(!IsAlive);
    }
}