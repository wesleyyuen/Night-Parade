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
    [SerializeField] private Sprite threeQuartersHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite quarterHeart;
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
        if (data1 != null) DisplayHeartsHelper(data1.SavedPlayerHealth, data1.SavedMaxPlayerHealth, saveSlot1Hearts);
        // Save Slot 2
        if (data2 != null) DisplayHeartsHelper(data2.SavedPlayerHealth, data2.SavedMaxPlayerHealth, saveSlot2Hearts);
        // Save Slot 3
        if (data3 != null) DisplayHeartsHelper(data3.SavedPlayerHealth, data3.SavedMaxPlayerHealth, saveSlot3Hearts);
    }

    void DisplayHeartsHelper (int currHealth, int maxHealth, Image[] hearts) {
        int numOfFullHearts = currHealth / 4;
        int maxHearts = maxHealth / 4;

        // Display hearts
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = (i < numOfFullHearts) ? fullHeart : emptyHeart;
            hearts[i].enabled = i < maxHearts;
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