using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public int currentBalance;
    public int nightEarnings =0;

    public void AddMoney(int amount)
    {
        Debug.Log($"Money added: {amount}");
        currentBalance += amount;
        nightEarnings += amount;
    }
    public int CalculateDrinkPayout(int basePrice, float accuracy)
    {
        int value = Mathf.RoundToInt(basePrice * accuracy);

        // add NPC tip
        OrderData order = GameStateManager.Instance.orderManager.selectedOrder;
        float tipPercentage = 0;
        switch (order.npc.personalityTag)
        {
            case PersonalityTag.Polite:
                break;
            case PersonalityTag.Patient:
                break;
            case PersonalityTag.Impatient:
                tipPercentage = (-0.1f * (1 - order.percentTimeLeft) + 0.05f); // guranteed 5% tip starts at 15% decreases with time taken
                break;
        }


        //add upgrade modifiers when made




        //adding tip - percentage as float e.g. 0.05 = 5% tip
        value += Mathf.RoundToInt(value * tipPercentage);

        //add some effects to show money added and tip given with satisfaction indicator.


        AddMoney(value);
        return value;
    }
    public void ResetNightEarnings()
    {
        nightEarnings = 0;
    }
    public bool CheckRentSuccess(int rentAmount)
    {
        if(rentAmount <= currentBalance)
        {
            currentBalance -= rentAmount;
            Debug.Log("Rent successful");
            return true;
        }
        else
        {
            Debug.Log("Rent fail");
            return false;
        }
        
    }
}
