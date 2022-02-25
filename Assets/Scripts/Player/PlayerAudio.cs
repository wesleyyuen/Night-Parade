using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioEvent softFootstepSFX;
    [SerializeField] private AudioEvent hardFootstepSFX;
    private PlayerAnimations _anim;
    private AudioSource _source;

    private void Awake()
    {
        _anim = GetComponentInParent<PlayerAnimations>();
        _source = GetComponent<AudioSource>();
    }
    
    public void PlayFootstepSFX()
    {
        if (GameMaster.Instance.currentScene == "Forest_Cave")
            hardFootstepSFX.Play(_source);
        else
            softFootstepSFX.Play(_source);

        _anim.CreateDustTrail();
    }

    public void PlayLandingSFX()
    {
        if (GameMaster.Instance.currentScene == "Forest_Cave")
            hardFootstepSFX.Play(_source);
        else
            softFootstepSFX.Play(_source);

        _anim.CreateDustTrail();
    }
}
