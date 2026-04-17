using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTarget : MonoBehaviour, IDropHandler
{
    public DrinkAssemblyController controller;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<DragInteraction>())
        {
            DragInteraction ingredientData = eventData.pointerDrag.GetComponent<DragInteraction>();

            controller.SetIngredient(ingredientData.ingredient);
            controller.AddStep();
        }
    }
}
