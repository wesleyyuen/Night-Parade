using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;
using MEC;
using TMPro;
using DG.Tweening;

// Class that has commonly used methods
public class Utility : MonoBehaviour
{
    private static Utility instance;
    public static Utility Instance {
        get  {return instance; }
    }

    private void Awake()
    {
        // Init DOTween
        DOTween.Init();
    }

    public static IEnumerator<float> _ChangeVariableAfterDelay<T>(Action<T> variable, float delay, T initialVal, T endVal)
    {
        variable(initialVal);
        yield return Timing.WaitForSeconds(delay);
        variable(endVal);
    }

    // public void SlowTimeForSeconds(float scale, float delay)
    // {
    //     StartCoroutine(_SlowTimeForSeconds(scale, delay));
    // }

    public static IEnumerator _SlowTimeForSeconds(float scale, float delay)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
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

    // Used by Tilemap
    public static void FadeGameObjectRecursively(GameObject obj, float from, float to, float fadingTime, bool isRecursive = true)
    {
        if (fadingTime == 0) {
            SetAlphaRecursively(obj, to);
            return;
        }

        Timing.RunCoroutine(Utility.FadeGameObject(obj, from, to, fadingTime));

        if (isRecursive && obj.transform.childCount > 0) {
            foreach (Transform child in obj.transform) {
                Utility.FadeGameObjectRecursively(child.gameObject, from, to, fadingTime);
            }
        }
    }

    public static IEnumerator<float> FadeGameObject(GameObject obj, float from, float to, float fadingTime)
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

        for (float t = 0f; t < 1f; t += Timing.DeltaTime / fadingTime) {
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
                
            yield return Timing.WaitForOneFrame;
        }
    }

    public static void FadeAreaText(TextMeshProUGUI text)
    {
        Timing.RunCoroutine(_FadeTextInAndOut(text, 3f, 1f).CancelWith(text.gameObject));
    }

    public static IEnumerator<float> _FadeTextInAndOut(TextMeshProUGUI text, float showingTime, float fadingTime)
    {
        text.gameObject.SetActive(true);

        for (float t = 0f; t < 1f; t += Timing.DeltaTime / fadingTime) {
            text.color = new Color (text.color.r, text.color.g, text.color.b, Mathf.SmoothStep (0f, 1f, t));
            yield return Timing.WaitForOneFrame;
        }

        // Display text
        yield return Timing.WaitForSeconds(showingTime);

        for (float t = 0f; t < 1f; t += Timing.DeltaTime / fadingTime) {
            text.color = new Color (text.color.r, text.color.g, text.color.b, Mathf.SmoothStep (1f, 0f, t));
            yield return Timing.WaitForOneFrame;
        }

        text.gameObject.SetActive(false);
    }

    public static IEnumerator<float> FadeImage(Image image, float from, float to, float fadingTime)
    {
        for (float t = 0f; t < 1f; t += Timing.DeltaTime / fadingTime) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.SmoothStep(from, to, t));
            yield return Timing.WaitForOneFrame;
        }
    }

    public static IEnumerator<float> ScaleGameObject(GameObject obj, Vector3 from, Vector3 to, float scalingTime)
    {
        if (scalingTime == 0) {
            obj.transform.localScale = to;
            yield break;
        }


        obj.transform.localScale = from;
        for (float t0 = 0f, t1 = 0f, t2 = 0f;
            t0 < 1f;
            t0 += Timing.DeltaTime / scalingTime, t1 += Timing.DeltaTime / scalingTime, t2 += Timing.DeltaTime / scalingTime) {
            obj.transform.localScale = new Vector3(Mathf.SmoothStep(from.x, to.x, t0),
                                                   Mathf.SmoothStep(from.y, to.y, t1),
                                                   Mathf.SmoothStep(from.z, to.z, t2));

            yield return Timing.WaitForOneFrame;
        }

        obj.transform.localScale = to;
    }

    public static void EnablePlayerControl(bool enable, float time = 0)
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        WeaponFSM weapon = FindObjectOfType<WeaponFSM>();
        player.GetComponent<PlayerAnimations>().EnablePlayerTurning(enable, time);
        player.GetComponent<PlayerMovement>().EnablePlayerMovement(enable, time);
        player.GetComponent<PlayerAbilityController>().EnableAbility(PlayerAbilityController.Ability.Jump , enable, time);
        weapon.EnablePlayerCombat(enable, time);
        weapon.EnablePlayerBlocking(enable, time);
    }

    public static void FreezePlayer(float time)
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
                WeaponFSM weapon = FindObjectOfType<WeaponFSM>();
        player.GetComponent<PlayerAnimations>().EnablePlayerTurning(false, time);
        player.GetComponent<PlayerAnimations>().FreezePlayerAnimation(time);
        player.GetComponent<PlayerMovement>().FreezePlayerPosition(time);
        player.GetComponent<PlayerAbilityController>().EnableAbility(PlayerAbilityController.Ability.Jump , false, time);
        weapon.EnablePlayerCombat(false, time);
        weapon.EnablePlayerBlocking(false, time);
    }

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }
}