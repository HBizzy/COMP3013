using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
        float acc = accuracy;
        foreach (KeyValuePair<Upgrade, float> upgrade in GameStateManager.Instance.upgradeManager.activeModifiers)
        {
            if (upgrade.Key.upgradeType == UpgradeType.MistakeForgiveness)
            {
                acc = Mathf.RoundToInt(upgrade.Key.valueModifier * acc);
                if (accuracy <= 0.75f && acc >= 0.75f)
                {
                    acc = 0.75f;
                }
            }
        }
        int value = Mathf.RoundToInt(basePrice * acc);

        // add NPC tip
        OrderData order = GameStateManager.Instance.orderManager.selectedOrder;
        float tipPercentage = 0;
        float timeLeft = order.percentTimeLeft;
        foreach (KeyValuePair<Upgrade, float> upgrade in GameStateManager.Instance.upgradeManager.activeModifiers)
        {
            if (upgrade.Key.upgradeType == UpgradeType.PatienceBoost)
            {
                timeLeft = Mathf.RoundToInt(upgrade.Key.valueModifier * timeLeft);
                if(order.percentTimeLeft <=0.8f && timeLeft >= 0.8f)
                {
                    timeLeft = 0.8f;
                }
            }
        }

        switch (order.npc.personalityTag)
        {     case PersonalityTag.Impatient:
                tipPercentage = (-0.1f * (1 - timeLeft) + 0.05f); // guranteed 5% tip starts at 15% decreases with time taken
                break;
            case PersonalityTag.Perfectionist:
                tipPercentage = (0.15f * accuracy - 0.05f); // guranteed 15% tip starts at 15% decreases accuracy
                break;
            case PersonalityTag.Generous:
                tipPercentage = (0.125f); // guranteed 12.5%
                break;
            case PersonalityTag.Cheap:
                tipPercentage = (0.05f); // 5% tip
                break;
            case PersonalityTag.Picky:
                tipPercentage = ((0.5f * accuracy + 0.5f * timeLeft) * 0.15f); // average of time percentage and accuracy percentage
                break;
            case PersonalityTag.Regular:
                tipPercentage = (0.1f + GameStateManager.Instance.orderManager.ordersCompleted *0.01f); // will tip more for the more customers have been served
                break;
        }

        tipPercentage = Mathf.Clamp(value, 0.0f, 0.25f);

        if (GameStateManager.Instance.reputationManager.reputation < 40)
        {
            tipPercentage *= 0.8f;
        }
        else if (GameStateManager.Instance.reputationManager.reputation > 60)
        {
            tipPercentage *= 1.2f;
        }

        //adding tip - percentage as float e.g. 0.05 = 5% tip
        value += Mathf.RoundToInt(value * tipPercentage);

        foreach (KeyValuePair<Upgrade,float> upgrade in GameStateManager.Instance.upgradeManager.activeModifiers)
        {
            if (upgrade.Key.upgradeType == UpgradeType.AlcoholQuality)
            {
                value = Mathf.RoundToInt(upgrade.Key.valueModifier * value);
            }
        }

        AddMoney(value);
        GameStateManager.Instance.orderManager.feedbackManager.SpawnFloatingMoneyText(value);
        return value;
    }
    public void ResetNightEarnings()
    {
        nightEarnings = 0;
    }
    public bool CheckRentSuccess(int rentAmount)
    {
        int rentDue = rentAmount;
        foreach(KeyValuePair<Upgrade, float> upgrade in GameStateManager.Instance.upgradeManager.activeModifiers)
        {
            if(upgrade.Key.upgradeType == UpgradeType.RentReduction)
            {
                rentDue = Mathf.RoundToInt(upgrade.Key.valueModifier * rentDue);
            }
        }
        if(rentDue <= currentBalance)
        {
            currentBalance -= rentDue;
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
