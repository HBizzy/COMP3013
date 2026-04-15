using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NightManager : MonoBehaviour
{
    public float nightDuration;
    public float currentTimeRemaining;
    public bool isNightRunning;
    public bool ticketsRemaining;

    public void BeginNight()
    {
        isNightRunning = true;
        currentTimeRemaining = nightDuration;
        if(GameStateManager.Instance.night %4 == 0)
        {
            GameStateManager.Instance.orderManager.AddNewRecipe();
        }
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
    }

    public void Update()
    {
        currentTimeRemaining = Mathf.Clamp(currentTimeRemaining, 0, nightDuration);
        if (isNightRunning)
        {
            UpdateTimer();
        }
        if(currentTimeRemaining <= 0 && isNightRunning)
        {
            OnTimerExpired();
        }
       
    }
 
}
