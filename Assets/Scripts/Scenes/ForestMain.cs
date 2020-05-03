using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestMain : MonoBehaviour
{
    public Transform player;
    public Transform tanukiSpawnPoint;

    // Use this for initialization
    public void Start()
    {
        GameMaster gameMaster = FindObjectOfType<GameMaster>();
        FindObjectOfType<GameMaster>().UpdateCurrentScene();
        if (gameMaster.getPrevScene() == "Forest_miniboss_tanuki")
        {
            player.position = tanukiSpawnPoint.position;
            player.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}