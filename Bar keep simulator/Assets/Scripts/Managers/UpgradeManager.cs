using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> purchasedUpgrades;
    public Dictionary<Upgrade,float> activeModifiers = new Dictionary<Upgrade, float> { };
    public bool PurchaseUpgrade(Upgrade upgrade)
    {

        if(GameStateManager.Instance.economyManager.currentBalance >= upgrade.cost && !purchasedUpgrades.Contains(upgrade))
        {
            GameStateManager.Instance.economyManager.AddMoney(-upgrade.cost);
            activeModifiers.Add(upgrade, upgrade.valueModifier);
            purchasedUpgrades.Add(upgrade);
            return true;
        }
        return false;
    }
    public bool IsUpgradeUnlocked(Upgrade upgrade)
    {
        if (activeModifiers.ContainsKey(upgrade))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public void ResetUpgrades()
    {
        activeModifiers = new Dictionary<Upgrade, float>();
    }
}
