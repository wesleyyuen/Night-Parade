using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonTutorial : MonoBehaviour
{
    [SerializeField] float duration;
    TextMeshPro text;
    KeyIcon icon;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        icon = GetComponentInChildren<KeyIcon>();
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player")) {
            icon.Fade(true, duration);
            StartCoroutine(Utility.FadeText(text, 0f, 1f, duration));
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag ("Player")) {
            icon.Fade(false, duration);
            StartCoroutine(Utility.FadeText(text, 1f, 0f, duration));
        }
    }
}