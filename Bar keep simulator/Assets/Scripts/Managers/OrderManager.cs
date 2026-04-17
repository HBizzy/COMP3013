using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public List<OrderData> currentOrders = new List<OrderData> { };
    public OrderData selectedOrder;
    public List<DrinkRecipe> availableRecipes;
    public List<DrinkRecipe> AllAvailableRecipes;
    public int ordersCompleted;
    public bool isOrderActive;
    public event Action<OrderData> OnOrderGenerated;
    public event Action<OrderData> OnOrderRemoved;
    public event Action OnOrderUpdated;
    public List<NPCType> possibleNPCs;

    private List<Wave> waves = new List<Wave>() {new Wave(5,2.0f,30.0f), new Wave(8, 1.8f, 25.0f), new Wave(12, 1.5f, 20.0f) } ;

    public void StartOrderGeneration()
    {
        StartCoroutine(RunWaves());
        StartCoroutine(CheckMinimumOrders());
    }
    public void GenerateOrder()
    {
        if (GameStateManager.Instance.nightManager.isNightRunning)
        {
            
            if (currentOrders.Count < 4)
            {
                DrinkRecipe newDrinkRecipe = availableRecipes[UnityEngine.Random.Range(0, availableRecipes.Count)];
                OrderData newDrink = new OrderData();
                newDrink.targetRecipe = newDrinkRecipe;
                NPCType npcChosen = possibleNPCs[UnityEngine.Random.Range(0, possibleNPCs.Count)];

                newDrink.npc = npcChosen;

                currentOrders.Add(newDrink);
                UnityEngine.Debug.Log($"Order Added: {newDrink.targetRecipe.drinkName}");
                
                OnOrderGenerated?.Invoke(newDrink);
            }
        }
        
    
    }
    public void SelectOrder(OrderData selectedRecipe)
    {
        selectedOrder = selectedRecipe;
        OnOrderUpdated?.Invoke();
    }
    public DrinkRecipe GetCurrentOrder()
    {
        return selectedOrder.targetRecipe;
    }
    public void SubmitDrink(List<DrinkStep> playerSteps)
    {
        EvaluateDrink(playerSteps);
    }
    public void EvaluateDrink(List<DrinkStep> playerSteps)
    {
        if (selectedOrder == null)
            return;

        if (playerSteps != null)
        {
            selectedOrder.submittedDrink = playerSteps;
            UnityEngine.Debug.Log("Drink Evaluation Start");
            //string recipeSteps = string.Join(",", selectedOrder.targetRecipe.steps.Select(t => $"{t.drinkIngredient.ingredientName}"));
            //Debug.Log($"Recipe: {recipeSteps}");
            //string playerStepsNames = string.Join(",", playerSteps.Select(t => $"{t.drinkIngredient.ingredientName}"));
            //Debug.Log($"Recipe: {playerStepsNames}");
            if (selectedOrder != null)
            {
                float currentAccuracy = 0;

                //accuracy, 50% for correct steps present, 50% for correct order, -10% per wrong step above recipe step count 
                int requiredSteps = selectedOrder.targetRecipe.steps.Count;
                int correctSteps = 0;
                int additionalErrorSteps = 0;
                List<DrinkStep> correctedSteps = new List<DrinkStep>();
                foreach (DrinkStep step in playerSteps)
                {
                    if (selectedOrder.targetRecipe.steps.Contains(step))
                    {
                        correctSteps++;
                        correctedSteps.Add(step);
                    }
                    else
                    {
                        additionalErrorSteps++;
                    }
                }
                currentAccuracy += (float)correctSteps / (float)requiredSteps * 0.6f;
                UnityEngine.Debug.Log($"Contents correct: {(float)correctSteps}/{(float)requiredSteps}");
                int lastMatchedIndex = -1;
                float correctOrderCount = 0;
                foreach (DrinkStep step in correctedSteps)
                {
                    int recipeIndex = selectedOrder.targetRecipe.steps.IndexOf(step);

                    if (recipeIndex > lastMatchedIndex)
                    {
                        correctOrderCount++;
                        lastMatchedIndex = recipeIndex;
                    }
                    else
                    {
                        continue;
                    }
                }
                currentAccuracy += correctOrderCount / (float)selectedOrder.targetRecipe.steps.Count * 0.4f;
                UnityEngine.Debug.Log($"Steps in order count: {correctOrderCount}/{(float)selectedOrder.targetRecipe.steps.Count}");
                currentAccuracy -= (float)additionalErrorSteps * 0.1f;
                UnityEngine.Debug.Log($"Extra steps: {additionalErrorSteps}");
                UnityEngine.Debug.Log($"Final accuracy: {currentAccuracy}");
                selectedOrder.accuracy = currentAccuracy;
                CompleteOrder(currentAccuracy);
            }
        
        }
    }
    public void CompleteOrder(float accuracy)
    {
        if (HasActiveOrder())
        {
            GameStateManager.Instance.economyManager.CalculateDrinkPayout(selectedOrder.targetRecipe.basePrice, accuracy);
            ordersCompleted++;
        }
       ClearOrder();
    }
    public void ClearOrder()
    {
        currentOrders.Remove(selectedOrder);
        OnOrderRemoved?.Invoke(selectedOrder);
        selectedOrder = null;

        if (!GameStateManager.Instance.nightManager.isNightRunning
            && currentOrders.Count == 0)
        {
            UnityEngine.Debug.Log("Night ending no orders left");
            GameStateManager.Instance.EndNight();
        }
    }
    public void removeOrder(OrderData drink)
    {
        currentOrders.Remove(drink);
        OnOrderRemoved?.Invoke(selectedOrder);
        if (!GameStateManager.Instance.nightManager.isNightRunning
            && currentOrders.Count == 0)
        {
            UnityEngine.Debug.Log("Night ending no orders left");
            GameStateManager.Instance.EndNight();
        }
    }
    public bool HasActiveOrder()
    {
        return selectedOrder != null;
    }

    public void AddNewRecipe()
    {
        List<DrinkRecipe> CanAdd = new List<DrinkRecipe> { };
        foreach(DrinkRecipe recipe in AllAvailableRecipes)
        {
            if (!availableRecipes.Contains(recipe))
            {
                CanAdd.Add(recipe);
            }
        }
        if(CanAdd.Count > 0)
        {
            availableRecipes.Add(CanAdd[UnityEngine.Random.RandomRange(0,availableRecipes.Count)]);
        }
    }

    public IEnumerator RunWaves()
    {
        foreach (Wave wave in waves)
        {
            if (!GameStateManager.Instance.nightManager.isNightRunning)
                yield break;

            for (int i = 0; i < wave.count; i++)
            {
                if (!GameStateManager.Instance.nightManager.isNightRunning)
                    yield break;

                if (currentOrders.Count < 4)
                {
                    GenerateOrder();
                }

                yield return new WaitForSeconds(wave.rate);
            }

            yield return new WaitForSeconds(wave.breakTime);
        }
    }

    public IEnumerator CheckMinimumOrders()
    {
        while (GameStateManager.Instance.nightManager.isNightRunning)
        {
            if (currentOrders.Count < 2)
            {
                GenerateOrder();
            }

            yield return new WaitForSeconds(UnityEngine.Random.RandomRange(1f, 3f));
        }
    }
}

public struct Wave
{
    public int count;
    public float rate;
    public float breakTime;

    public Wave(int count, float rate, float breakTime)
    {
        this.count = count;
        this.rate = rate;    
        this.breakTime = breakTime;
    }
}