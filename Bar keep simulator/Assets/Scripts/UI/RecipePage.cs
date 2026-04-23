using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipePage : MonoBehaviour
{
    public GameObject instructionPrefab;
    public GameObject content;

    public DrinkRecipe recipe;
    public List<GameObject> instructions = new List<GameObject>() { };

    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI recipeDescription;
    public TextMeshProUGUI recipePrice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BindNewPageData(DrinkRecipe recipe)
    {
        ResetInstructions();

        recipeName.text = recipe.drinkName;
        recipeDescription.text = recipe.description;
        recipePrice.text = $"Price: £{recipe.basePrice}";

        int recipeIndex = 1;
        foreach(DrinkStep step in recipe.steps)
        {
            GameObject newInstruction = Instantiate(instructionPrefab, content.transform);
            instructions.Add(newInstruction);

            newInstruction.GetComponent<InstructionPart>().Bind(recipeIndex, step);
            recipeIndex++;
        }
    }
    public void ResetInstructions()
    {
        foreach(GameObject instruction in instructions)
        {
            Destroy(instruction);
        }
    }

    public void MakeBlankPage()
    {
        ResetInstructions();
        recipeName.text = "";
        recipeDescription.text = "";
        recipePrice.text = $"";
    }
}
