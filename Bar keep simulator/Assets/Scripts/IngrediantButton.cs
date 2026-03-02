using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngrediantButton : MonoBehaviour
{
    public DrinkIngredient ingredient;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {

            this.GetComponentInParent<DrinkAssemblyController>().SetIngredient(ingredient);
            // send ingredient to manager

        });
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
