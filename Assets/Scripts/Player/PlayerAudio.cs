using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {

    public void FootstepSFX () {
        AudioManager.Instance.Play ("Forest_Footsteps");
    }
}
