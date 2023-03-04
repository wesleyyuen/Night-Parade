using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using MEC;
using TMPro;
using DG.Tweening;

// Class that has commonly used methods
public class Utility : MonoBehaviour
{
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

    public static void SetAlphaRecursively(GameObject obj, float alpha, bool isRecursive = true)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        UnityEngine.Rendering.Universal.Light2D light = obj.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
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

    private static IEnumerator<float> FadeGameObject(GameObject obj, float from, float to, float fadingTime)
    {
        if (fadingTime == 0) {
            SetAlphaRecursively(obj, to);
            yield break;
        }

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        UnityEngine.Rendering.Universal.Light2D light = obj.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
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

    private static IEnumerator<float> _FadeTextInAndOut(TextMeshProUGUI text, float showingTime, float fadingTime)
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

    public static void EnablePlayerControl(bool enable, float time = 0, bool shouldFreezeAnim = false)
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        WeaponFSM weapon = FindObjectOfType<WeaponFSM>();
        if (player.TryGetComponent<PlayerAnimations>(out PlayerAnimations animations)) {
            animations.EnablePlayerTurning(enable, time);
            if (shouldFreezeAnim)
            {
                animations.FreezePlayerAnimation(!enable, time);
            }
        }
        if (player.TryGetComponent<PlayerMovement>(out PlayerMovement movement)) {
            movement.EnablePlayerMovement(enable, time);
        }
        if (player.TryGetComponent<PlayerAbilityController>(out PlayerAbilityController abilities)) {
            abilities.EnableAbility(PlayerAbilityController.Ability.All, enable, time);
        }
        weapon.EnablePlayerCombat(enable, time);
        weapon.EnablePlayerBlocking(enable, time);
    }

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }
}