using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReputationManager : MonoBehaviour
{
    public int reputation = 50;

    public void AddReputation(int rep)
    {
        reputation += rep;

        reputation = Mathf.Clamp(reputation, 0, 100);

        if(rep < 0)
        {
            GameStateManager.Instance.orderManager.feedbackManager.SpawnFloatingRepText(false);
        }
        else
        {
            GameStateManager.Instance.orderManager.feedbackManager.SpawnFloatingRepText(true);
        }

    }
}
