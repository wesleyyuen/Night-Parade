using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    PlayerAnimations _anim;

    void Awake()
    {
        _anim = GetComponentInParent<PlayerAnimations>();
    }
    
    public void PlayFootstepSFX()
    {
        if (GameMaster.Instance.currentScene == "Forest_Cave")
            SoundManager.Instance.Play("Forest_Hard_Footsteps");
        else
            SoundManager.Instance.Play("Forest_Soft_Footsteps");

        _anim.CreateDustTrail();
    }
}
