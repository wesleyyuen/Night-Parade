using System.Collections.Generic;
using UnityEngine;
using MEC;

[CreateAssetMenu(fileName = "AmbientSO", menuName = "ScriptableObjects/Audio/Ambient")]
public class AmbientAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    public RangeFloat volume = new RangeFloat(0.5f, 0.5f);

    [MinMaxRange(0f, 2f)]
    public RangeFloat pitch = new RangeFloat(1f, 1f);
    public float fadeDuration;

    public override void Play(AudioSource source = null)
    {
        source = source ?? AudioManager.Instance.GetAmbientAudioSource();

        source.clip = clips[Random.Range(0, clips.Length)];
        source.loop = true;
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);

        if (fadeDuration > 0f) {
            Timing.RunCoroutine(_FadeVolume(source, 0f, source.volume, fadeDuration));
        }
        
        source.Play();
    }

    private IEnumerator<float> _FadeVolume(AudioSource source, float from, float to, float duration)
    {
        source.volume = from;
        for (float t = 0f; t < 1f; t += Time.deltaTime / duration) {
            source.volume = Mathf.SmoothStep(from, to, t);
            yield return Timing.WaitForOneFrame;
        }
        source.volume = to;
    }
}