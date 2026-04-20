using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OrderTicket : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI orderName;
    public Image timer;
    public DrinkRecipe order;
    public Image highlight;
    public NPCType npc;
    public float timeLeft =9999999;
    public float maxTime;

    public OrderTicketUIController controller;

    private bool timeRunning = false;
    public OrderData Order;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {

            GameStateManager.Instance.orderManager.SelectOrder(Order);
            controller.ResetAllTicketHighlights();
            if (highlight != null)
            {
                highlight.gameObject.SetActive(true);
            }

        });
    }

    // Update is called once per frame
    void Update()
    {

        if (timeRunning)
        {
            timeLeft -= Time.deltaTime;
            timer.fillAmount = timeLeft / maxTime;
            if (GameStateManager.Instance.orderManager.selectedOrder != null)
                GameStateManager.Instance.orderManager.selectedOrder.percentTimeLeft = timeLeft / maxTime;
            if (timeLeft / maxTime <= 0.25f)
            {
                gameObject.GetComponent<Animator>().enabled = true;
            }
            {
                if (timeLeft <= 0)
                {
                    GameStateManager.Instance.orderManager.selectedOrder = null;
                    timeRunning = false;
                    GameStateManager.Instance.orderManager.removeOrder(Order);
                    controller.feedbackManager.OrderTimeOut();
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void Bind(OrderData Order)
    {
        this.Order = Order;
        order = Order.targetRecipe;
        orderName.text = order.drinkName;
        this.npc = Order.npc;
        generateTicketData();
    }

    public void generateTicketData()
    {
        timeLeft = UnityEngine.Random.Range(npc.minPatienceTime, npc.maxPatienceTime);

        foreach (KeyValuePair<Upgrade, float> upgrade in GameStateManager.Instance.upgradeManager.activeModifiers)
        {
            if (upgrade.Key.upgradeType == UpgradeType.TicketTimeBoost)
            {
                timeLeft = Mathf.RoundToInt(upgrade.Key.valueModifier * timeLeft);
            }
        }
        maxTime = timeLeft;
        timer.fillAmount = 1.0f;
        timeRunning = true;
    }

    public void ResetHighlight()
    {
        if (highlight != null)
        {
            highlight.gameObject.SetActive(false);
        }
    }
}
