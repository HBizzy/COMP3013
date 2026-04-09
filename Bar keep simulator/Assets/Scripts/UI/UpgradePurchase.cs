using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePurchase : MonoBehaviour
{
    public TextMeshProUGUI nameDesc;
    public TextMeshProUGUI price;
    public Button buyButton;
    public Upgrade upgradeSO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bind(Upgrade upgrade , UpgradeUIManager manager)
    {
        upgradeSO = upgrade;

        nameDesc.text = $"{upgrade.name} - {upgrade.description}";
        price.text = $"{upgrade.cost}g"; // change to red or green if they can purchase or not.

        //add button on click listener to link back to upgrade manager

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            manager.TryPurchase(upgradeSO);
        });
    }
}
