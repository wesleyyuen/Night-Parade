using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    PlayerAnimations _anim;

    private void Awake()
    {
        _anim = GetComponentInParent<PlayerAnimations>();
    }
    
    public void PlayFootstepSFX()
    {
        if (GameMaster.Instance.currentScene == "Forest_Cave")
            SoundManager.Instance.PlayOnce("Forest_Hard_Footsteps");
        else
            SoundManager.Instance.PlayOnce("Forest_Soft_Footsteps");

        _anim.CreateDustTrail();
    }

    public void PlayLandingSFX()
    {
        if (GameMaster.Instance.currentScene == "Forest_Cave")
            SoundManager.Instance.PlayOnce("Forest_Hard_Footsteps", 2f);
        else
            SoundManager.Instance.PlayOnce("Forest_Soft_Footsteps", 2f);

        _anim.CreateDustTrail();
    }
}
