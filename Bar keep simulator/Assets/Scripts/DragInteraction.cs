using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragInteraction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DrinkIngredient ingredient;
    public GameObject floatingIconPrefab;
    public GameObject floatingIcon;
    public GameObject canvas;
    public Sprite icon;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //create floating icon (prefab with alpha lower) and set floating icons image to icon
        if(floatingIconPrefab != null)
        {
            floatingIcon = Instantiate(floatingIconPrefab, canvas.transform);
            floatingIcon.GetComponent<Image>().sprite =icon;
            floatingIcon.GetComponent<Image>().raycastTarget = false;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (floatingIcon != null)
        {
            floatingIcon.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(floatingIcon);
        floatingIcon = null;
    }

}
