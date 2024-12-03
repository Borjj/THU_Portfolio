using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    private Material material;
    [SerializeField] private float fadeSpeed = 2f;
    private float currentAlpha = 0f;
    private float targetAlpha = 0f;

// ---------------------------------------------------------------------------- //

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        SetAlpha(0f);
    }

    void Update()
    {
        if (currentAlpha != targetAlpha)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            SetAlpha(currentAlpha);
        }
    }

// ---------------------------------------------------------------------------- //

    public void Reveal()
    {
        targetAlpha = 1f;
    }

    public void Hide()
    {
        targetAlpha = 0f;
    }

    private void SetAlpha(float alpha)
    {
        Color color = material.color;
        color.a = alpha;
        material.color = color;
    }

}
