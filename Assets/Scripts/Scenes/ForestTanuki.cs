using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTanuki : MonoBehaviour
{
    public Transform player;

    // Use this for initialization
    public void Start()
    {
        GameMaster gameMaster = FindObjectOfType<GameMaster>();
        FindObjectOfType<GameMaster>().UpdateCurrentScene();
        if (gameMaster.getPrevScene() == "Forest_Main")
        {
            player.position = new Vector2(-59f, 8.5f);
        }
    }
}