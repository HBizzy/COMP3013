using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirLogic : MonoBehaviour
{
    public RectTransform centerTransform;

    public float totalRotation = 0f;
    public float requiredRotation = 1080f; // 3 circles

    public float minRadius = 50f;
    public float maxRadius = 300f;

    private float lastAngle;
    private bool isActive = false;

    public System.Action OnStirComplete;

    public void StartStir(Vector2 startMousePos)
    {
        isActive = true;
        totalRotation = 0f;

        lastAngle = GetAngle(startMousePos);
    }

    public void StopStir()
    {
        isActive = false;
    }

    public void ProcessInput(Vector2 mousePos)
    {
        if (!isActive) return;

        Vector2 center = centerTransform.position;

        float distance = Vector2.Distance(mousePos, center);

        // Ignore if too close or too far
        if (distance < minRadius || distance > maxRadius)
            return;

        float currentAngle = GetAngle(mousePos);
        float deltaAngle = currentAngle - lastAngle;

        // Fix wraparound
        if (deltaAngle > 180f) deltaAngle -= 360f;
        if (deltaAngle < -180f) deltaAngle += 360f;

        totalRotation += deltaAngle;
        lastAngle = currentAngle;

        if (Mathf.Abs(totalRotation) >= requiredRotation)
        {
            CompleteStir();
        }
    }

    private float GetAngle(Vector2 mousePos)
    {
        Vector2 center = centerTransform.position;
        Vector2 direction = (mousePos - center).normalized;

        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private void CompleteStir()
    {
        isActive = false;
        OnStirComplete?.Invoke();
    }
}