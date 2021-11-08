using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;

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

    public static IEnumerator ChangeVariableAfterDelay<T>(Action<T> variable, float delay, T initialVal, T endVal)
    {
        variable(initialVal);
        yield return new WaitForSeconds(delay);
        variable(endVal);
    }

    public static IEnumerator ChangeVariableAfterDelayInRealTime<T>(Action<T> variable, float delay, T initialVal, T endVal)
    {
        variable(initialVal);
        yield return new WaitForSecondsRealtime(delay);
        variable(endVal);
    }

    public static IEnumerator SlowTimeForSeconds(float scale, float delay)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public static IEnumerator DoCoroutineAfterSeconds(float time, IEnumerator coroutine)
    {
        yield return new WaitForSecondsRealtime(time);
        StaticCoroutine.Start(coroutine);
    }

    public static void SetAlphaRecursively(GameObject obj, float alpha, bool isRecursive = true)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Light2D light = obj.GetComponent<Light2D>();
        ParticleSystem particles = obj.GetComponent<ParticleSystem>();
        TMP_Text text = obj.GetComponent<TMP_Text>();
        Image image = obj.GetComponent<Image>();
        Tilemap tilemap = obj.GetComponent<Tilemap>();

        // Sprite Renderer
        if (sr != null)
            sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, alpha);

        // Light
        if (light != null)
            light.color = new Color (light.color.r, light.color.g, light.color.b, alpha);

        // Particle System
        if (particles != null) {
            ParticleSystem.MainModule main = particles.main;
            Color color = new Color (main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, alpha);
            main.startColor = new ParticleSystem.MinMaxGradient(color);
        }

        // Text
        if (text != null)
            text.color = new Color (text.color.r, text.color.g, text.color.b, alpha);

        // Image
        if (image != null)
            image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);

        // Tilemap
        if (tilemap != null)
            tilemap.color = new Color (tilemap.color.r, tilemap.color.g, tilemap.color.b, alpha);
                
        if (isRecursive && obj.transform.childCount > 0) {
            foreach (Transform child in obj.transform) {
                Utility.SetAlphaRecursively(child.gameObject, alpha);
            }
        }
    }

    public static void FadeGameObjectRecursively(GameObject obj, float from, float to, float fadingTime, bool isRecursive = true)
    {
        if (fadingTime == 0) {
            SetAlphaRecursively(obj, to);
            return;
        }

        StaticCoroutine.Start(Utility.FadeGameObject(obj, from, to, fadingTime));

        if (isRecursive && obj.transform.childCount > 0) {
            foreach (Transform child in obj.transform) {
                Utility.FadeGameObjectRecursively(child.gameObject, from, to, fadingTime);
            }
        }
    }

    public static IEnumerator FadeGameObject(GameObject obj, float from, float to, float fadingTime)
    {
        if (fadingTime == 0) {
            SetAlphaRecursively(obj, to);
            yield break;
        }

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Light2D light = obj.GetComponent<Light2D>();
        ParticleSystem particles = obj.GetComponent<ParticleSystem>();
        TMP_Text text = obj.GetComponent<TMP_Text>();
        Image image = obj.GetComponent<Image>();
        Tilemap tilemap = obj.GetComponent<Tilemap>();

        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            // Sprite Renderer
            if (sr != null)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.SmoothStep(from, to, t));

            // Light
            if (light != null)
                light.color = new Color(light.color.r, light.color.g, light.color.b, Mathf.SmoothStep(from, to, t));

            // Particle System
            if (particles != null) {
                ParticleSystem.MainModule main = particles.main;
                Color color = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, Mathf.SmoothStep(from, to, t));
                main.startColor = new ParticleSystem.MinMaxGradient(color);
            }

            // Text
            if (text != null)
                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.SmoothStep(from, to, t));

            // Image
            if (image != null)
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.SmoothStep(from, to, t));

            // Tilemap
            if (tilemap != null)
                tilemap.color = new Color (tilemap.color.r, tilemap.color.g, tilemap.color.b, Mathf.SmoothStep(from, to, t));
                
            yield return null;
        }
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
        yield return new WaitForSeconds(showingTime);

        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            text.color = new Color (text.color.r, text.color.g, text.color.b, Mathf.SmoothStep (1f, 0f, t));
            yield return null;
        }
        text.enabled = false;
    }

    public static IEnumerator FadeImage(Image image, float from, float to, float fadingTime)
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.SmoothStep(from, to, t));
            yield return null;
        }
    }

    public static IEnumerator ScaleGameObject(GameObject obj, Vector3 from, Vector3 to, float scalingTime)
    {
        if (scalingTime == 0) {
            obj.transform.localScale = to;
            yield break;
        }


        obj.transform.localScale = from;
        for (float t0 = 0f, t1 = 0f, t2 = 0f;
            t0 < 1f;
            t0 += Time.deltaTime / scalingTime, t1 += Time.deltaTime / scalingTime, t2 += Time.deltaTime / scalingTime) {
            obj.transform.localScale = new Vector3(Mathf.SmoothStep(from.x, to.x, t0),
                                                   Mathf.SmoothStep(from.y, to.y, t1),
                                                   Mathf.SmoothStep(from.z, to.z, t2));

            yield return null;
        }

        obj.transform.localScale = to;
    }

    public static void EnablePlayerControl(bool enable, float time = 0)
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
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

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }
}