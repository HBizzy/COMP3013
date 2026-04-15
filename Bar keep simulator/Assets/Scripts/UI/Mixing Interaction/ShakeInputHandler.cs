using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShakeInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public ShakeLogic logic;

    private bool isHolding = false;
    private Vector2 lastMousePosition;

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        lastMousePosition = eventData.position;

        logic.StartShake();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;

        logic.StopShake();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isHolding) return;

        Vector2 currentMousePosition = eventData.position;
        Vector2 delta = currentMousePosition - lastMousePosition;
        lastMousePosition = currentMousePosition;

        logic.ProcessInput(delta);

        MoveWithMouse(eventData);
    }

    private void MoveWithMouse(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );

        rectTransform.localPosition = localPoint;
    }
}