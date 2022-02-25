using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SaveSlotMenu
{
    public Button button;
    public TextMeshProUGUI mons;
    public Image[] hearts;
    public TextMeshProUGUI time;
}

public class LoadMenu : MonoBehaviour
{
    [SerializeField] private SaveSlotMenu saveSlot1;
    [SerializeField] private SaveSlotMenu saveSlot2;
    [SerializeField] private SaveSlotMenu saveSlot3;
    [SerializeField] private Sprite fullHeart;

    private PlayerData data1;
    private PlayerData data2;
    private PlayerData data3;

    private void Awake()
    {
        // TODO if File exists
        data1 = SaveManager.Load(1);
        data2 = SaveManager.Load(2);
        data3 = SaveManager.Load(3);
        saveSlot1.button.gameObject.SetActive(data1 != null);
        saveSlot2.button.gameObject.SetActive(data2 != null);
        saveSlot3.button.gameObject.SetActive(data3 != null);

        DisplayHearts();
        DisplayMons();
        DisplayPlayTime();
    }

    private void DisplayHearts()
    {
        // Save Slot 1
        if (data1 != null) DisplayHeartsHelper(data1.CurrentHealth, data1.MaxHealth, saveSlot1.hearts);
        // Save Slot 2
        if (data2 != null) DisplayHeartsHelper(data1.CurrentHealth, data2.MaxHealth, saveSlot2.hearts);
        // Save Slot 3
        if (data3 != null) DisplayHeartsHelper(data1.CurrentHealth, data3.MaxHealth, saveSlot3.hearts);
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
        if (data1 != null) saveSlot1.mons.text = data1.CoinsOnHand.ToString();
        if (data2 != null) saveSlot2.mons.text = data2.CoinsOnHand.ToString();
        if (data3 != null) saveSlot3.mons.text = data3.CoinsOnHand.ToString();
    }

    private void DisplayPlayTime()
    {
        if (data1 != null) saveSlot1.time.text = SecondsToStringHelper(data1.PlayTime);
        if (data2 != null) saveSlot2.time.text = SecondsToStringHelper(data2.PlayTime);
        if (data3 != null) saveSlot3.time.text = SecondsToStringHelper(data3.PlayTime);
    }

    string SecondsToStringHelper(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600F);
        int minutes = Mathf.FloorToInt ((time % 3600F) / 60F);
        return string.Format("{0:00}:{1:00}", hours, minutes);
    }
}