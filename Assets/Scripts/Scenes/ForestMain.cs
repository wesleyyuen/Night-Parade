using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestMain : MonoBehaviour
{
    public Transform player;

    // Use this for initialization
    public void Start()
    {
        GameMaster gameMaster = FindObjectOfType<GameMaster>();
        FindObjectOfType<GameMaster>().UpdateCurrentScene();
        if (gameMaster.getPrevScene() == "Forest_miniboss_tanuki")
        {
            player.position = new Vector2(230f, 12.5f);
            player.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}