using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour {

    [SerializeField] private Button saveSlot1;
    [SerializeField] private TextMeshProUGUI saveSlot1Mon;
    [SerializeField] private Image[] saveSlot1Hearts;
    [SerializeField] private TextMeshProUGUI saveSlot1Time;
    [SerializeField] private Button saveSlot2;
    [SerializeField] private TextMeshProUGUI saveSlot2Mon;
    [SerializeField] private Image[] saveSlot2Hearts;
    [SerializeField] private TextMeshProUGUI saveSlot2Time;
    [SerializeField] private Button saveSlot3;
    [SerializeField] private TextMeshProUGUI saveSlot3Mon;
    [SerializeField] private Image[] saveSlot3Hearts;
    [SerializeField] private TextMeshProUGUI saveSlot3Time;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private PlayerData data1;
    private PlayerData data2;
    private PlayerData data3;

    void Awake () {
        // TODO if File exists
        data1 = SaveManager.Load (1);
        data2 = SaveManager.Load (2);
        data3 = SaveManager.Load (3);
        saveSlot1.gameObject.SetActive (data1 != null);
        saveSlot2.gameObject.SetActive (data2 != null);
        saveSlot3.gameObject.SetActive (data3 != null);

        DisplayHearts ();
        DisplayMons ();
        DisplayPlayTime ();
    }

    void DisplayHearts () {
        // Save Slot 1
        if (data1 != null) {
            for (int i = 0; i < saveSlot1Hearts.Length; i++) {
                saveSlot1Hearts[i].sprite = (i < data1.SavedPlayerHealth) ? fullHeart : emptyHeart;
                saveSlot1Hearts[i].enabled = i < data1.SavedMaxPlayerHealth;
            }
        }
        // Save Slot 2
        if (data2 != null) {
            for (int i = 0; i < saveSlot2Hearts.Length; i++) {
                saveSlot2Hearts[i].sprite = (i < data2.SavedPlayerHealth) ? fullHeart : emptyHeart;
                saveSlot2Hearts[i].enabled = i < data2.SavedMaxPlayerHealth;
            }
        }
        // Save Slot 3
        if (data3 != null) {
            for (int i = 0; i < saveSlot3Hearts.Length; i++) {
                saveSlot3Hearts[i].sprite = (i < data3.SavedPlayerHealth) ? fullHeart : emptyHeart;
                saveSlot3Hearts[i].enabled = i < data3.SavedMaxPlayerHealth;
            }
        }
    }

    void DisplayMons () {
        if (data1 != null) saveSlot1Mon.text = data1.SavedPlayerCoinsOnHand.ToString ();
        if (data2 != null) saveSlot2Mon.text = data2.SavedPlayerCoinsOnHand.ToString ();
        if (data3 != null) saveSlot3Mon.text = data3.SavedPlayerCoinsOnHand.ToString ();
    }

    void DisplayPlayTime () {
        if (data1 != null) saveSlot1Time.text = SecondsToStringHelper (data1.SavedPlayTimeInSecs);
        if (data2 != null) saveSlot2Time.text = SecondsToStringHelper (data2.SavedPlayTimeInSecs);
        if (data3 != null) saveSlot3Time.text = SecondsToStringHelper (data3.SavedPlayTimeInSecs);
    }

    string SecondsToStringHelper (float time) {
        int hours = Mathf.FloorToInt (time / 3600F);
        int minutes = Mathf.FloorToInt ((time % 3600F) / 60F);
        return string.Format ("{0:00}:{1:00}", hours, minutes);
    }
}