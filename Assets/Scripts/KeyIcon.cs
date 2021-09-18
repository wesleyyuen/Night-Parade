using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyIcon : MonoBehaviour
{
    [SerializeField] TextMeshPro key;
    [SerializeField] string keyString;

    void Awake()
    {
        key.text = keyString.ToUpper();
    }
}
