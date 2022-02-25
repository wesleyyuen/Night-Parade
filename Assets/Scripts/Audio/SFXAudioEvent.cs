using UnityEngine;

[CreateAssetMenu(fileName = "SFXSO", menuName = "ScriptableObjects/Audio/SFX")]
public class SFXAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    public RangeFloat volume = new RangeFloat(0.5f, 0.5f);

    [MinMaxRange(0f, 2f)]
    public RangeFloat pitch = new RangeFloat(1f, 1f);

    public override void Play(AudioSource source = null)
    {
        source = source ?? AudioManager.Instance.GetSFXAudioSource();

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        source.PlayOneShot(source.clip);
    }
}