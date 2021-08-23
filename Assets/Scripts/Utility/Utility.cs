using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class that has commonly used methods
public class Utility : MonoBehaviour
{
    // By CykesDev
    // https://forum.unity.com/threads/c-coroutines-in-static-functions.134546/
    // Enable coroutines from non-MonoBehaviour scripts
    public class StaticCoroutine : MonoBehaviour {
        private static StaticCoroutine _instance;
    
        private void OnDestroy()
        { _instance.StopAllCoroutines(); }
    
        private void OnApplicationQuit()
        { _instance.StopAllCoroutines(); }
    
        private static StaticCoroutine Build() {
            if (_instance != null) return _instance;
    
            _instance = (StaticCoroutine)FindObjectOfType(typeof(StaticCoroutine));
    
            if (_instance != null) return _instance;
    
            GameObject instanceObject = new GameObject("StaticCoroutine");
            instanceObject.AddComponent<StaticCoroutine>();
            _instance = instanceObject.GetComponent<StaticCoroutine>();
    
            if (_instance != null) return _instance;
    
            Debug.LogError("Build did not generate a replacement instance. Method Failed!");
    
            return null;
        }
    
        public static void Start(string methodName)
        { Build().StartCoroutine(methodName); }
        public static void Start(string methodName, object value)
        { Build().StartCoroutine(methodName, value); }
        public static void Start(IEnumerator routine)
        { Build().StartCoroutine(routine); }
    }

    public static void FadeAreaText(TextMeshProUGUI text)
    {
        StaticCoroutine.Start(FadeTextInAndOut(text, 3f, 1f));
    }

    public static IEnumerator FadeTextInAndOut(TextMeshProUGUI text, float showingTime, float fadingTime)
    {
        text.enabled = true;
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            text.color = new Color (text.color.r, text.color.g, text.color.b, Mathf.SmoothStep (0f, 1f, t));
            yield return null;
        }

        // Display text
        yield return new WaitForSeconds (showingTime);

        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            text.color = new Color (text.color.r, text.color.g, text.color.b, Mathf.SmoothStep (1f, 0f, t));
            yield return null;
        }
        text.enabled = false;
    }

    public static IEnumerator FadeSprite(SpriteRenderer sprite, float from, float to, float fadingTime)
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep (from, to, t));
            yield return null;
        }
    }

    public static IEnumerator FadeImage(Image image, float from, float to, float fadingTime)
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            image.color = new Color (image.color.r, image.color.g, image.color.b, Mathf.SmoothStep (from, to, t));
            yield return null;
        }
    }

    public static IEnumerator FadeText(TextMeshPro text, float from, float to, float fadingTime)
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            text.color = new Color (text.color.r, text.color.g, text.color.b, Mathf.SmoothStep (from, to, t));
            yield return null;
        }
    }

    public static IEnumerator ChangeVariableAfterDelay<T>(Action<T> variable, float delay, T initialVal, T endVal)
    {
        variable(initialVal);
        yield return new WaitForSecondsRealtime(delay);
        variable(endVal);
    }

    public static IEnumerator DoCoroutineAfterSeconds(float time, IEnumerator coroutine)
    {
        yield return new WaitForSecondsRealtime(time);
        StaticCoroutine.Start(coroutine);
    }

    public static void EnablePlayerControl(bool enable, float time = 0)
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        player.GetComponent<Animator>().SetFloat ("Horizontal", 0f);
        player.GetComponent<PlayerAnimations>().EnablePlayerTurning(enable, time);
        player.GetComponent<PlayerMovement>().EnablePlayerMovement(enable, time);
        player.GetComponentInChildren<WeaponFSM>().EnablePlayerCombat(enable, time);
        player.GetComponentInChildren<WeaponFSM>().EnablePlayerBlocking(enable, time);
        player.GetComponent<PlayerAbilityController>().EnableAbility(PlayerAbilityController.Ability.Jump , enable, time);
    }

    public static void FreezePlayer(float time)
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        player.GetComponent<PlayerAnimations>().EnablePlayerTurning(false, time);
        player.GetComponent<PlayerAnimations>().FreezePlayerAnimation(time);
        player.GetComponent<PlayerMovement>().FreezePlayerPosition(time);
        player.GetComponentInChildren<WeaponFSM>().EnablePlayerCombat(false, time);
        player.GetComponentInChildren<WeaponFSM>().EnablePlayerBlocking(false, time);
        player.GetComponent<PlayerAbilityController>().EnableAbility(PlayerAbilityController.Ability.Jump , false, time);
    }
}