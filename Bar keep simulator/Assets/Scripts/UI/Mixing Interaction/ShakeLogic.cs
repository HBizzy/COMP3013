using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeLogic : MonoBehaviour
{
    public float shakeProgress = 0f;
    public float maxProgress = 100f;

    public float speedThreshold = 50f;
    public float progressRate = 40f;
    public float decayRate = 20f;

    private Vector2 previousDirection;
    private bool hasPrevious = false;

    private bool isActive = false;

    public System.Action OnShakeComplete;

    public void StartShake()
    {
        isActive = true;
        
    }
    public void ResetShake()
    {
        shakeProgress = 0f;
        hasPrevious = false;
    }
    public void StopShake()
    {
        isActive = false;
    }

    public void ProcessInput(Vector2 delta)
    {
        if (!isActive) return;

        float speed = delta.magnitude;

        if (speed > speedThreshold)
        {
            Vector2 direction = delta.normalized;

            if (hasPrevious)
            {
                float dot = Vector2.Dot(previousDirection, direction);

                if (dot < 0) // direction change
                {
                    shakeProgress += progressRate * 1.5f * Time.deltaTime;
                }
                else
                {
                    shakeProgress += progressRate * Time.deltaTime;
                }
            }

            previousDirection = direction;
            hasPrevious = true;
        }
        

        shakeProgress = Mathf.Clamp(shakeProgress, 0f, maxProgress);

        if (shakeProgress >= maxProgress)
        {
            CompleteShake();
        }
    }

    private void CompleteShake()
    {
        isActive = false;
        OnShakeComplete?.Invoke();
    }
    void Update()
    {
 

        shakeProgress -= decayRate * Time.deltaTime;
        shakeProgress = Mathf.Clamp(shakeProgress, 0f, maxProgress);
    }
}
