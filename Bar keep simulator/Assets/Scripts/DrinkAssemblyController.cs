using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DrinkAssemblyController : MonoBehaviour
{
    public DrinkIngredient selectedIngredient;
    public List<DrinkStep> currentSteps = new List<DrinkStep>();
    public Button drinkButton;
    public Button submitButton;
    public Button stirButton;
    public Button shakeButton;
    public DrinkStep stirStep;
    public DrinkStep shakeStep;

    public List<DrinkStep> availableSteps;
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.orderManager.OnOrderUpdated += clearStepsAndIngredient;
        drinkButton.onClick.RemoveAllListeners();
        drinkButton.onClick.AddListener(() => {

            AddStep();

        });
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(() => {

            GameStateManager.Instance.orderManager.SubmitDrink(currentSteps);
            clearStepsAndIngredient();

        });
        stirButton.onClick.RemoveAllListeners();
        stirButton.onClick.AddListener(() => {

            AddStep(stirStep);

        });
        shakeButton.onClick.RemoveAllListeners();
        shakeButton.onClick.AddListener(() => {

            AddStep(shakeStep);

        });
    }
    private void OnDisable()
    {
        GameStateManager.Instance.orderManager.OnOrderUpdated -= clearStepsAndIngredient;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetIngredient(DrinkIngredient ingredient)
    {
        if(ingredient != null)
        {
            selectedIngredient = ingredient;
        }
    }

    public void AddStep()
    {
        if(selectedIngredient != null)
        {
            foreach(DrinkStep step in availableSteps)
            {
                if(step.drinkIngredient == selectedIngredient)
                {
                    currentSteps.Add(step);
                    Debug.Log("Step added");
                }
            }
        }
        selectedIngredient = null;
    }

    public void AddStep(DrinkStep step)
    {
        currentSteps.Add(step);
        
        selectedIngredient = null;
    }
    public void clearStepsAndIngredient()
    {
        selectedIngredient = null;
        currentSteps.Clear();
    }

}
