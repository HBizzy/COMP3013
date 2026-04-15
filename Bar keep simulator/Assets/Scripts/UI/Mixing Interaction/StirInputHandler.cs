using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StirInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public StirLogic logic;

    private bool isHolding = false;
    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 grabOffset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        grabOffset = rectTransform.localPosition - new Vector3(localPoint.x,localPoint.y,0.0f);

        logic.StartStir(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        logic.StopStir();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isHolding) return;

        MoveWithMouse(eventData);
        logic.ProcessInput(eventData.position);
    }

    private void MoveWithMouse(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        Vector2 targetPos = localPoint + grabOffset;

        rectTransform.localPosition = Vector2.Lerp(
            rectTransform.localPosition,
            targetPos,
            15f * Time.deltaTime
        );
    }
}