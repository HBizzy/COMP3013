using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class FeedbackManager : MonoBehaviour
{
    public GameObject FeedbackPrefab;

    private List<string> perfectMsgs = new List<string>() 
    {"Flawless.",
    "That's perfection.",
    "Exactly what I wanted.",
    "Masterpiece.",
    "Spot on.",
    "Couldn't be better.",
    "You nailed it.",
    "Bar-quality brilliance." };

    private List<string> goodMsgs = new List<string>()
   {
    "Pretty good.",
    "Nice work.",
    "That'll do nicely.",
    "Tastes great.",
    "Well made.",
    "Solid drink.",
    "I like it.",
    "Good balance."
    };

    private List<string> alrightMsgs = new List<string>
    {
    "It's okay.",
    "Not bad.",
    "Could be better.",
    "A bit off.",
    "Decent, I guess.",
    "I've had better.",
    "Close, but not quite.",
    "Needs some work."
    };
    private List<string> badMsgs = new List<string>
    {
    "What is this?",
    "That's rough.",
    "No thanks.",
    "Did you even try?",
    "Way off.",
    "That's terrible.",
    "I'm not drinking that.",
    "Completely wrong."
    };

    private List<string> customerLeft = new List<string>
    {
    "I don't have all day.",
    "This is taking too long.",
    "I'm leaving.",
    "Forget it.",
    "Way too slow.",
    "I've been waiting forever.",
    "I'll get a drink somewhere else.",
    "You lost me.",
    "Next time, be quicker.",
    "Not worth the wait.",
    "I'm done waiting.",
    "Guess I'm not getting served."
    };

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OrderTimeOut()
    {
        RectTransform parentRect = GetComponent<RectTransform>();

        Rect r = parentRect.rect;

        float x = Random.Range(r.xMin, r.xMax);
        float y = Random.Range(r.yMin, r.yMax);

        GameObject text = Instantiate(FeedbackPrefab, parentRect);

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(x, y);

        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.color = UnityEngine.Color.red;
        tmp.text = customerLeft[Random.RandomRange(0, customerLeft.Count)];

    }

    public void FeedbackFromAccuracy(float accuracy)
    {
        string message = "";
        UnityEngine.Color color = UnityEngine.Color.black;
        if(accuracy >= 1)
        {
            message = perfectMsgs[Random.RandomRange(0,perfectMsgs.Count)];
            color = UnityEngine.Color.blue;
        }
        else if(accuracy >= 0.75f)
        {
            message = goodMsgs[Random.RandomRange(0, goodMsgs.Count)];
            color = UnityEngine.Color.green;
        }
        else if (accuracy >= 0.5f)
        {
            message = alrightMsgs[Random.RandomRange(0, alrightMsgs.Count)];
            color = UnityEngine.Color.yellow;
        }
        else if (accuracy < 0.5f)
        {
            message = badMsgs[Random.RandomRange(0, badMsgs.Count)];
            color = UnityEngine.Color.red;
        }

        RectTransform parentRect = GetComponent<RectTransform>();

        Rect r = parentRect.rect;

        float x = Random.Range(r.xMin, r.xMax);
        float y = Random.Range(r.yMin, r.yMax);

        GameObject text = Instantiate(FeedbackPrefab, parentRect);

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(x, y);

        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.color = color;
        tmp.text = message;
    }

    public void SpawnFloatingMoneyText(float amount)
    {
        UnityEngine.Color color = UnityEngine.Color.white;
        if(amount > 0)
        {
            color = UnityEngine.Color.green;
        }
        else
        {
            color = UnityEngine.Color.red;
            amount *= -1;
        }

        RectTransform parentRect = GetComponent<RectTransform>();

        Rect r = parentRect.rect;

        float x = Random.Range(r.xMin, r.xMax);
        float y = Random.Range(r.yMin, r.yMax);

        GameObject text = Instantiate(FeedbackPrefab, parentRect);

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(x, y);

        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.color = color;
        tmp.text = $"£{amount}";
    }

    public void SpawnFloatingRepText(bool positive)
    {
        UnityEngine.Color color = UnityEngine.Color.white;
        string msg = "";
        if (positive)
        {
            color = UnityEngine.Color.green;
            msg = "+Rep";
        }
        else
        {
            color = UnityEngine.Color.red;
            msg = "-Rep";
        }

        RectTransform parentRect = GetComponent<RectTransform>();

        Rect r = parentRect.rect;

        float x = Random.Range(r.xMin, r.xMax);
        float y = Random.Range(r.yMin, r.yMax);

        GameObject text = Instantiate(FeedbackPrefab, parentRect);

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(x, y);

        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.color = color;
        tmp.text = msg;
    }
}
