using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeUI : MonoBehaviour
{
    public ShakeLogic logic;
    public Slider progress;

    void Update()
    {
        float progressPercent = logic.shakeProgress / logic.maxProgress;
        progress.maxValue = 1.0f;
        progress.value = progressPercent;
    }
}

