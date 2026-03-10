using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public List<DrinkRecipe> currentOrders = new List<DrinkRecipe> { };
    public DrinkRecipe selectedOrder;  //change to order class to hold reference to npc, time limit and tip modifier
    public List<DrinkRecipe> availableRecipes;
    public int ordersCompleted;
    public bool isOrderActive;
    public event Action<DrinkRecipe> OnOrderGenerated;
    public event Action<DrinkRecipe> OnOrderRemoved;
    public event Action OnOrderUpdated;
    public void GenerateOrder()
    {
        if (GameStateManager.Instance.nightManager.isNightRunning)
        {
            StartCoroutine(WaitForRandomTime());
            if (currentOrders.Count < 4)
            {
                DrinkRecipe newDrink = availableRecipes[UnityEngine.Random.RandomRange(0, availableRecipes.Count)];
                currentOrders.Add(newDrink);
                Debug.Log($"Order Added: {newDrink.drinkName}");
                string recipeSteps = string.Join(",", newDrink.steps.Select(t => $"{t.drinkIngredient.ingredientName}"));
                Debug.Log($"Recipe: {recipeSteps}");

                OnOrderGenerated?.Invoke(newDrink);
            }
        }
        else { StopAllCoroutines(); }
    
    }
    public void SelectOrder(DrinkRecipe selectedRecipe)
    {
        selectedOrder = selectedRecipe;
        OnOrderUpdated?.Invoke();
    }
    public DrinkRecipe GetCurrentOrder()
    {
        return selectedOrder;
    }
    public void SubmitDrink(List<DrinkStep> playerSteps)
    {
        EvaluateDrink(playerSteps);
    }
    public void EvaluateDrink(List<DrinkStep> playerSteps)
    {
        Debug.Log("Drink Evaluation Start");
        string recipeSteps = string.Join(",", selectedOrder.steps.Select(t => $"{t.drinkIngredient.ingredientName}"));
        Debug.Log($"Recipe: {recipeSteps}");
        string playerStepsNames = string.Join(",", playerSteps.Select(t => $"{t.drinkIngredient.ingredientName}"));
        Debug.Log($"Recipe: {playerStepsNames}");
        if (selectedOrder != null)
        {
            float currentAccuracy = 0;

            //accuracy, 50% for correct steps present, 50% for correct order, -10% per wrong step above recipe step count 
            int requiredSteps = selectedOrder.steps.Count;
            int correctSteps = 0;
            int additionalErrorSteps = 0;
            List<DrinkStep> correctedSteps = new List<DrinkStep>();
            foreach (DrinkStep step in playerSteps)
            {
                if (selectedOrder.steps.Contains(step))
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
            Debug.Log($"Contents correct: {(float)correctSteps}/{(float)requiredSteps}");
            int lastMatchedIndex = -1;
            float correctOrderCount = 0;
            foreach (DrinkStep step in correctedSteps)
            {
                int recipeIndex = selectedOrder.steps.IndexOf(step);

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
            currentAccuracy += correctOrderCount / (float)selectedOrder.steps.Count * 0.4f;
            Debug.Log($"Steps in order count: {correctOrderCount}/{(float)selectedOrder.steps.Count}");
            currentAccuracy -= (float)additionalErrorSteps * 0.1f;
            Debug.Log($"Extra steps: {additionalErrorSteps}");
            Debug.Log($"Final accuracy: {currentAccuracy}");
            CompleteOrder(currentAccuracy);
        }
    }
    public void CompleteOrder(float accuracy)
    {
        if (HasActiveOrder())
        {
            GameStateManager.Instance.economyManager.CalculateDrinkPayout(selectedOrder.basePrice, accuracy);
            ordersCompleted++;
        }
       ClearOrder();
    }
    public void ClearOrder()
    {
        currentOrders.Remove(selectedOrder);
        OnOrderRemoved?.Invoke(selectedOrder);
        selectedOrder = null;
    }
    public bool HasActiveOrder()
    {
        return selectedOrder != null;
    }

    public IEnumerator WaitForRandomTime()
    {
        yield return new WaitForSeconds(UnityEngine.Random.RandomRange(10,20));
        GenerateOrder();
    }
}
