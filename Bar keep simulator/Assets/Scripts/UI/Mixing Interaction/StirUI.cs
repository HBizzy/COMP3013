using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StirUI : MonoBehaviour
{
    public StirLogic logic;
    public Slider progressBar;

    void Update()
    {
        float progress = Mathf.Abs(logic.totalRotation) / logic.requiredRotation;
        progressBar.maxValue = 1.0f;
        progressBar.value = progress;
    }
}
