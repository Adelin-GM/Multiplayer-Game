using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{
    public int MaxKills = 3;
    public GameObject GameOverPopup;
    public Text WinnerText;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(" kills: " + MaxKills);

        System.Random rnd = new System.Random();
        int x = rnd.Next(-10, 3);
        int z = rnd.Next(0, 15);

        PhotonNetwork.Instantiate("Multiplayer", new Vector3(x, 0, z), Quaternion.identity);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer.GetScore() == MaxKills)
        {
            Debug.Log("score: " + targetPlayer.GetScore() + " kills: " + MaxKills);
            WinnerText.text = targetPlayer.NickName;
            GameOverPopup.SetActive(true);
            StorePersonalBest();
        }
    }

    private void StorePersonalBest()
    {
        var currentScore = PhotonNetwork.LocalPlayer.GetScore();
        var playerData = GameManager.Instance.playerData;
        if (currentScore > playerData.beastScore)
        {
            playerData.username = PhotonNetwork.LocalPlayer.NickName;
            playerData.beastScore = currentScore;
            playerData.date = DateTime.UtcNow;
            playerData.totalPlayersInTheGame = PhotonNetwork.CurrentRoom.PlayerCount;
            playerData.roomName = PhotonNetwork.CurrentRoom.Name;

            GameManager.Instance.GlobalLeaderboard.SubmitScore(currentScore);
            GameManager.Instance.SavePlayerData();
        }
    }

    private void StoreRoundsWon()
    {
        var playerData = GameManager.Instance.playerData;
        Debug.Log($"My score is: {PhotonNetwork.LocalPlayer.GetScore()}");
        if (PhotonNetwork.LocalPlayer.GetScore() == MaxKills)
        {
            playerData.roundsWon++;
            Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} has score {PhotonNetwork.LocalPlayer.GetScore()} and has {playerData.roundsWon} rounds won");

            GameManager.Instance.RoundsLeaderboard.SubmitScore(playerData.roundsWon);
            GameManager.Instance.SavePlayerData();
        }
    }


    public void LeaveGame() 
    {
        StoreRoundsWon();
        
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }
}
