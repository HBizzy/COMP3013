using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookController : MonoBehaviour
{
    public RecipePage page1;
    public RecipePage page2;

    public Button Open;
    public Button Close;
    public Button Next;
    public Button Previous;

    public List<DrinkRecipe> recipes;
    public GameObject bookPanel;
    public int pageIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        recipes = GameStateManager.Instance.orderManager.AllAvailableRecipes;
        bookPanel.SetActive(false);
        ReBindPages();
        Open.onClick.RemoveAllListeners();
        Open.onClick.AddListener(() => 
        {
            bookPanel.SetActive(true);
            
        });
        Close.onClick.RemoveAllListeners();
        Close.onClick.AddListener(() =>
        {
            bookPanel.SetActive(false);
            
        });

        Next.onClick.RemoveAllListeners();
        Next.onClick.AddListener(() =>
        {
            MoveToNextPage();
        });

        Previous.onClick.RemoveAllListeners();
        Previous.onClick.AddListener(() =>
        {
            MoveToPreviousPage();
        });

    }

    // Update is called once per frame
    void Update()
    {
        if(pageIndex == 0)
        {
            Previous.interactable = false;
        }
        else
        {
            Previous.interactable = true;
        }
        if(pageIndex >= recipes.Count-1)
        {
            Next.interactable = false;
        }
        else
        {
            Next.interactable = true;
        }
    }
    public void MoveToNextPage()
    {
        if (pageIndex != recipes.Count || pageIndex != recipes.Count-1)
            pageIndex +=2;
        ReBindPages();
    }

    public void MoveToPreviousPage()
    {
        if(pageIndex != 0 || pageIndex!=1)
            pageIndex-=2;
        ReBindPages();
    }

    public void ReBindPages()
    {

        DrinkRecipe recipe1 = recipes[pageIndex];
        DrinkRecipe recipe2 = null;
        if(pageIndex + 1 < recipes.Count)
            recipe2 = recipes[pageIndex+1];

        page1.BindNewPageData(recipe1);

        if(recipe2 == null)
        {
            page2.MakeBlankPage();
        }
        else
        {
            page2.BindNewPageData(recipe2);
        }
    }
}
