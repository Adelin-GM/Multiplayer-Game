using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferencesPopup : MonoBehaviour
{
    public Toggle ShowEnemyNames;
    public Toggle ShowYourName;
    // Start is called before the first frame update
    void Start()
    {
        var playerData = GameManager.Instance.playerData;
        ShowEnemyNames.isOn = playerData.showEnemyNameTags;
        ShowYourName.isOn = playerData.showYourNameTag;
    }

    public void ToggleShowEnemyNameChanged()
    {
        var playerData = GameManager.Instance.playerData;
        playerData.showEnemyNameTags = ShowEnemyNames.isOn;
        GameManager.Instance.SavePlayerData();
    }

    public void ToggleShowYourNameChanged()
    {
        var playerData = GameManager.Instance.playerData;
        playerData.showYourNameTag = ShowYourName.isOn;
        GameManager.Instance.SavePlayerData();
    }
}
