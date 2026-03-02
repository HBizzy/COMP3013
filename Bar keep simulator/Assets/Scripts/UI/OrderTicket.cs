using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OrderTicket : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI orderName;
    public DrinkRecipe order;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {

            GameStateManager.Instance.orderManager.SelectOrder(order);
            

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Bind(DrinkRecipe Order)
    {
        order = Order;
        orderName.text = order.drinkName;
    }
}
