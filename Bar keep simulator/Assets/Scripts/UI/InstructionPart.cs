using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPart : MonoBehaviour
{
    public TextMeshProUGUI stepText;
    public Image icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bind(int stepNum, DrinkStep step)
    {
        stepText.text = $"{stepNum}) {step.instructionText}";

        icon.sprite = step.icon;
    }
}
