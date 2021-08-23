using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyIcon : MonoBehaviour
{
    [SerializeField] SpriteRenderer backer;
    [SerializeField] TextMeshPro key;
    [SerializeField] string keyString;

    void Awake()
    {
        key.text = keyString.ToUpper();
        backer.color = new Color(backer.color.r, backer.color.g, backer.color.b, 0f);
        key.color = new Color(key.color.r, key.color.g, key.color.b, 0f);
    }

    public void Fade(bool fadeIn, float duration)
    {
        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;
        StartCoroutine(Utility.FadeSprite(backer, from, to, duration));
        StartCoroutine(Utility.FadeText(key, from, to, duration));
    }
}
