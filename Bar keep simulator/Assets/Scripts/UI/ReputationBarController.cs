using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationBarController : MonoBehaviour
{
    public Slider repBar;
    public Image repBarColour;
    // Start is called before the first frame update
    void Start()
    {
        repBar.maxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        repBar.value = GameStateManager.Instance.reputationManager.reputation;

        float t = repBar.value / 100f;

        if (t < 0.5f)
        {
            float localT = t / 0.5f;
            repBarColour.color = Color.Lerp(Color.red, Color.grey, localT);
        }
        else
        {
            float localT = (t - 0.5f) / 0.5f;
            repBarColour.color = Color.Lerp(Color.grey, Color.green, localT);
        }
    }
}
