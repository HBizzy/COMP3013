using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeUIManager : MonoBehaviour
{
    public List<Upgrade> availableUpgrades;
    public GameObject UIObj;

    public GameObject Content;
    public Dictionary<Upgrade,GameObject> activeUIObjects = new Dictionary<Upgrade, GameObject> { };

    public TextMeshProUGUI moneyAvailable;
     // Start is called before the first frame update
    void Start()
    {
        moneyAvailable.text = $"Money available: £{GameStateManager.Instance.economyManager.currentBalance}";
        availableUpgrades.Sort((a, b) => a.cost.CompareTo(b.cost));
        foreach (Upgrade upgrade in availableUpgrades)
        {
            GameObject itemToAdd = Instantiate(UIObj, Content.transform);
            activeUIObjects.Add(upgrade, itemToAdd);
            itemToAdd.GetComponent<UpgradePurchase>().Bind(upgrade, this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TryPurchase(Upgrade upgrade)
    {
        if (GameStateManager.Instance.upgradeManager.PurchaseUpgrade(upgrade))
        {
            Destroy(activeUIObjects[upgrade]);
        }
        
    }
}
