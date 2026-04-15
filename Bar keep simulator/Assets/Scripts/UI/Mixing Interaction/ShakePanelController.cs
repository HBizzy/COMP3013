using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine;

public class ShakePanelController : MonoBehaviour
{
    public GameObject panel;
    public ShakeLogic logic;

    private void Start()
    {
        logic.OnShakeComplete += HandleComplete;
        panel.SetActive(false);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
        logic.ResetShake();
        logic.StartShake();
    }

    private void HandleComplete()
    {
        panel.SetActive(false);
        UnityEngine.Debug.Log("Shake Complete!");
    }
}