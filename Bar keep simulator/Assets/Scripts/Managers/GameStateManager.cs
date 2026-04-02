using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
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

    public event Action OnHubEnter;

    //testing
    [Header("Testing")]
    public DrinkRecipe testRecipe;
    public List<DrinkStep> perfectSteps;
    public List<DrinkStep> mediumSteps;
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
                SceneManager.LoadScene("MenuScene");
                break;
            case GameState.NightActive:
                SceneManager.LoadScene("NightScene");
                break;
            case GameState.NightEnd:
                SceneManager.LoadScene("HubScene");
                StartCoroutine(updateReviewText());
                break;
            case GameState.FailState:
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartNight();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EndNight();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            orderManager.GenerateOrder();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OrderData newOrder = new OrderData();
            newOrder.targetRecipe = testRecipe;
            orderManager.selectedOrder = newOrder;
            orderManager.SubmitDrink(perfectSteps);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OrderData newOrder = new OrderData();
            newOrder.targetRecipe = testRecipe;
            orderManager.selectedOrder = newOrder;
            orderManager.SubmitDrink(mediumSteps);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            economyManager.AddMoney(99);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            economyManager.CheckRentSuccess(1000);
        }
    }
    public IEnumerator delayStartGenerateOrder()
    {
        yield return new WaitForSeconds(1.0f);
        orderManager.GenerateOrder();
    }
    public IEnumerator updateReviewText()
    {
        yield return new WaitForSeconds(0.1f);
        OnHubEnter?.Invoke();
    }
}

public enum GameState
{
    MainMenu,
    NightActive,
    NightEnd,
    FailState,
}