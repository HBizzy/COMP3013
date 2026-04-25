using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    private GameState currentState;
    private GameState previousState;
    public int night = 0;
    public NightManager nightManager;
    public OrderManager orderManager;
    public EconomyManager economyManager;
    public UpgradeManager upgradeManager;
    public ReputationManager reputationManager;
    public event Action OnHubEnter;

    public event Action OnNightStart;
    public AudioMixer mixer;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        InitializeGame();
    }

    public void InitializeGame()
    {
        currentState = GameState.MainMenu;
    }
    public void RequestStateChange(GameState state)
    {

    }

    public void ChangeState(GameState state)
    {
        OnStateExited(currentState);
        previousState = currentState;
        currentState = state;
        Debug.Log($"State changed: {previousState} -> {currentState}");
        OnStateEntered(state);
    }
    public void OnStateEntered(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                ResetAudioEffects();
                SceneManager.LoadScene("MenuScene");
                break;
            case GameState.NightActive:
                ResetAudioEffects();
                SceneManager.LoadScene("NightScene");
                OnNightStart?.Invoke();
                break;
            case GameState.NightEnd:
                ResetAudioEffects();
                SceneManager.LoadScene("HubScene");
                StartCoroutine(updateReviewText());
                break;
            case GameState.FailState:
                ResetAudioEffects();
                SceneManager.LoadScene("MenuScene");
                break;

        }
    }
    public void loadHub()
    {
        ChangeState(GameState.NightEnd);
    }
    public void OnStateExited(GameState state)
    {
        //clean up data of current scene
    }
    public void StartNight()
    {
        orderManager.currentOrders.Clear();
        orderManager.selectedOrder = null;
        nightManager.isNightRunning = false;
        ChangeState(GameState.NightActive);
        night += 1;
        orderManager.ordersCompleted = 0;
        economyManager.nightEarnings = 0;
        nightManager.BeginNight();
        StartCoroutine(delayStartGenerateOrder());
        
    }
    public void EndNight()
    {
        ChangeState(GameState.NightEnd);
        if (!economyManager.CheckRentSuccess(Mathf.RoundToInt(night / 4) * 500)) 
        {
            TriggerFailState();
        }
        orderManager.currentOrders.Clear();
        orderManager.selectedOrder = null;
        

    }
    public void TriggerFailState()
    {
        ChangeState(GameState.FailState);
    }
    public GameState GetCurrentState()
    {
        return currentState;
    }
    public void Update()
    {
       
    }
    public IEnumerator delayStartGenerateOrder()
    {
        yield return new WaitForSeconds(1.0f);
        orderManager.StartOrderGeneration();
    }
    public IEnumerator updateReviewText()
    {
        yield return new WaitForSeconds(0.1f);
        OnHubEnter?.Invoke();
    }
    public void ResetAudioEffects()
    {
        mixer.SetFloat("LowPass", 22000f);
    }
}

public enum GameState
{
    MainMenu,
    NightActive,
    NightEnd,
    FailState,
}