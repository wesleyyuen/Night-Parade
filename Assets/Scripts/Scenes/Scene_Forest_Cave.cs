using TMPro;
using UnityEngine;

public class Scene_Forest_Cave : AreaScript
{
    enum SpawnPoint {
        NotTesting,
        Area1
    }
    [SerializeField] SpawnPoint _spawnPoint;
    [SerializeField] Transform area1SpawnPoint;
    [SerializeField] TextMeshProUGUI forestText;
    
    protected override void Start()
    {
        base.Start();

        // Handle BGM
        if (GameMaster.Instance.prevScene != "Forest_Area1")
            SoundManager.Instance.Play("Forest_Ambience");

        if (GameMaster.Instance.prevScene == "Main_Menu") {
            _spawnPoint = SpawnPoint.NotTesting;
            Utility.FadeAreaText(forestText);
        }
        else if (GameMaster.Instance.prevScene == "Forest_Area1"
            || _spawnPoint == SpawnPoint.Area1) {
            foreach (Transform child in _playerGroup) {
                child.position = area1SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }
        }
    }
}
