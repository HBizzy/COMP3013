using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightManager : MonoBehaviour
{
    public float nightDuration;
    public float currentTimeRemaining;
    public bool isNightRunning;

    public void BeginNight()
    {
        isNightRunning = true;
        currentTimeRemaining = nightDuration;
    }
    public void UpdateTimer()
    {
        currentTimeRemaining -= Time.deltaTime;
    }

    public void ForceEndNight()
    {
        isNightRunning = false;
        GameStateManager.Instance.TriggerFailState();

    }
    public bool IsNightActive()
    {
        return currentTimeRemaining > 0 && isNightRunning;
    }
    public void OnTimerExpired()
    {
        isNightRunning = false;
        GameStateManager.Instance.EndNight();
    }

    public void Update()
    {
        if (isNightRunning && isNightRunning)
        {
            UpdateTimer();
        }
        if(currentTimeRemaining <= 0 && isNightRunning)
        {
            OnTimerExpired();
        }
       
    }
}
