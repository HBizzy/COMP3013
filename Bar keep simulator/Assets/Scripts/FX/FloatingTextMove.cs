using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextMove : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float lifetime = 1.5f;

    private TextMeshProUGUI text;
    private Vector2 direction;
    private float timer;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        float xDir = Random.value < 0.5f ? -1f : 1f;
        direction = new Vector2(xDir, 1f).normalized;
    }

    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);

        Color c = text.color;
        c.a = alpha;
        text.color = c;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}