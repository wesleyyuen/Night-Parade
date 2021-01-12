using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Common : MonoBehaviour {
    // Class that has commonly used methods

    public static IEnumerator FadeText (TextMeshProUGUI text, float showingTime, float fadingTime) {
        text.enabled = true;
        // Fade in text
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            text.color = new Color (1, 1, 1, Mathf.Lerp (0f, 1f, t));
            yield return null;
        }
        // Display text
        yield return new WaitForSeconds (showingTime);
        // Fade out text
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            text.color = new Color (1, 1, 1, Mathf.Lerp (1f, 0f, t));
            yield return null;
        }
        text.enabled = false;
    }

    public static IEnumerator ChangeVariableAfterDelay<T>(Action<T> variable, float delay, T initialVal, T endVal) {
        variable(initialVal);
        yield return new WaitForSeconds(delay);
        variable(endVal);
    }
}