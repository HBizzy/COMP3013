using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StirPanelController : MonoBehaviour
{
    public GameObject panel;
    public StirLogic logic;

    private void Start()
    {
        logic.OnStirComplete += HandleComplete;
        panel.SetActive(false);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        logic.StartStir(Input.mousePosition); // initialise
    }

    private void HandleComplete()
    {
        panel.SetActive(false);
        UnityEngine.Debug.Log("Stir Complete!");
    }
}