using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    [SerializeField] Button saveSlot1;
    [SerializeField] TextMeshProUGUI saveSlot1Mon;
    [SerializeField] Image[] saveSlot1Hearts;
    [SerializeField] TextMeshProUGUI saveSlot1Time;
    [SerializeField] Button saveSlot2;
    [SerializeField] TextMeshProUGUI saveSlot2Mon;
    [SerializeField] Image[] saveSlot2Hearts;
    [SerializeField] TextMeshProUGUI saveSlot2Time;
    [SerializeField] Button saveSlot3;
    [SerializeField] TextMeshProUGUI saveSlot3Mon;
    [SerializeField] Image[] saveSlot3Hearts;
    [SerializeField] TextMeshProUGUI saveSlot3Time;
    [SerializeField] Sprite fullHeart;

    PlayerData data1;
    PlayerData data2;
    PlayerData data3;

    private void Awake()
    {
        // TODO if File exists
        data1 = SaveManager.Load(1);
        data2 = SaveManager.Load(2);
        data3 = SaveManager.Load(3);
        saveSlot1.gameObject.SetActive(data1 != null);
        saveSlot2.gameObject.SetActive(data2 != null);
        saveSlot3.gameObject.SetActive(data3 != null);

        DisplayHearts();
        DisplayMons();
        DisplayPlayTime();
    }

    private void DisplayHearts()
    {
        // Save Slot 1
        if (data1 != null) DisplayHeartsHelper(data1.CurrentHealth, data1.MaxHealth, saveSlot1Hearts);
        // Save Slot 2
        if (data2 != null) DisplayHeartsHelper(data2.CurrentHealth, data2.MaxHealth, saveSlot2Hearts);
        // Save Slot 3
        if (data3 != null) DisplayHeartsHelper(data3.CurrentHealth, data3.MaxHealth, saveSlot3Hearts);
    }

    private void DisplayHeartsHelper(int currHealth, int maxHealth, Image[] hearts)
    {
        int numOfFullHearts = currHealth / 4;
        int maxHearts = maxHealth / 4;

        // Display hearts
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = fullHeart;
            hearts[i].enabled = i < maxHearts;
        }
    }

    private void DisplayMons()
    {
        if (data1 != null) saveSlot1Mon.text = data1.CoinsOnHand.ToString ();
        if (data2 != null) saveSlot2Mon.text = data2.CoinsOnHand.ToString ();
        if (data3 != null) saveSlot3Mon.text = data3.CoinsOnHand.ToString ();
    }

    private void DisplayPlayTime()
    {
        if (data1 != null) saveSlot1Time.text = SecondsToStringHelper (data1.PlayTime);
        if (data2 != null) saveSlot2Time.text = SecondsToStringHelper (data2.PlayTime);
        if (data3 != null) saveSlot3Time.text = SecondsToStringHelper (data3.PlayTime);
    }

    string SecondsToStringHelper(float time)
    {
        int hours = Mathf.FloorToInt (time / 3600F);
        int minutes = Mathf.FloorToInt ((time % 3600F) / 60F);
        return string.Format ("{0:00}:{1:00}", hours, minutes);
    }
}