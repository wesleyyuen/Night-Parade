using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Common : MonoBehaviour {
    // Class that has commonly used methods

    public IEnumerator FadeText (GameObject text, float showingTime, float fadingTime) {
        text.SetActive (true);
        TextMeshProUGUI textMesh = text.GetComponent<TextMeshProUGUI> ();
        // Fade in text
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            textMesh.color = new Color (1, 1, 1, Mathf.Lerp (0f, 1f, t));
            yield return null;
        }
        // Display text
        yield return new WaitForSeconds (showingTime);
        // Fade out text
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            textMesh.color = new Color (1, 1, 1, Mathf.Lerp (1f, 0f, t));
            yield return null;
        }
        text.SetActive (false);
    }
}