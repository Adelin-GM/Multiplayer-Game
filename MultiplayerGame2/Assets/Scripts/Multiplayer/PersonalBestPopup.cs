using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalBestPopup : MonoBehaviour
{
    public GameObject ScoreHolder;
    public GameObject NoScoreText;
    public Text Username;
    public Text BestScore;
    public Text Date;
    public Text TotalPlayers;
    public Text RoomName;


    private void OnEnable()
    {
        UpdatePersonalBestUI();
    }


    public void UpdatePersonalBestUI()
    {
        var playerData = GameManager.Instance.playerData;
        if (playerData.username != null)
        {
            Username.text = playerData.username;
            BestScore.text = playerData.beastScore.ToString();
            Date.text = playerData.date.ToString();
            TotalPlayers.text = playerData.totalPlayersInTheGame.ToString();
            RoomName.text = playerData.roomName;

            ScoreHolder.SetActive(true);
            NoScoreText.SetActive(false);
        }
        else
        {
            ScoreHolder.SetActive(false);
            NoScoreText.SetActive(true);
        }
    }

}
