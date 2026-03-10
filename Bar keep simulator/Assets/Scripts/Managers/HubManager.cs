using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    public TextMeshProUGUI nightReviewText;
    public Button startNightButton;

    public void Awake()
    {
        startNightButton.onClick.RemoveAllListeners();
        startNightButton.onClick.AddListener(() =>
        {
            GameStateManager.Instance.StartNight();
        });
        GameStateManager.Instance.OnHubEnter += setReviewText;

    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnHubEnter -= setReviewText;
    }

    public void setReviewText()
    {
        if(GameStateManager.Instance.night != 0)
        {
            int currentBalance = GameStateManager.Instance.economyManager.currentBalance;
            int rent = Mathf.RoundToInt(GameStateManager.Instance.night / 4) * 500;
            int nightEarnings = GameStateManager.Instance.economyManager.nightEarnings;

            int startingMoney = currentBalance + rent - nightEarnings;
            nightReviewText.text = $"Night {GameStateManager.Instance.night} Results\r\n\r\nStarting Balance: £{startingMoney}\r\nMoney Earned: <color=green>+£{nightEarnings}</color>\r\nRent Deducted: <color=red>-£{rent}</color>\r\n-------------------------\r\nCurrent Balance: £{currentBalance}";
        }
        else
        {
            nightReviewText.text = $"Opening Night\r\n\r\nYour bar is ready to open.\r\n\r\nStarting Balance: £{1000}\r\n\r\nServe drinks, earn money, and make sure you can cover the nightly rent.\r\n\r\nPress \"Start Night\" to begin.";
        }
        
    }
}
