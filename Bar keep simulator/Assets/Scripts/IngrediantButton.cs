using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class IngrediantButton : MonoBehaviour
{
    public DrinkIngredient ingredient;
    public GameObject bottle;
    public bool isBottle = false;
    private Button button;
    private Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {

            this.GetComponentInParent<DrinkAssemblyController>().SetIngredient(ingredient);
            this.GetComponentInParent<DrinkAssemblyController>().playPickupSound();


        });
        if(isBottle)
            originalPos = bottle.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(this.GetComponentInParent<DrinkAssemblyController>() && this.GetComponentInParent<DrinkAssemblyController>().selectedIngredient == ingredient)
        {
            if(isBottle)
            {
                bottle.transform.position = new Vector3(originalPos.x, originalPos.y + 10, originalPos.z);
            }
        }
        else
        {
            if (isBottle)
                bottle.transform.position = new Vector3(originalPos.x, originalPos.y, originalPos.z);
        }
    }
}
