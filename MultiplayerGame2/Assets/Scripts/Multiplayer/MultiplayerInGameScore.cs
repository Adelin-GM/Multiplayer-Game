using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerInGameScore : MonoBehaviourPunCallbacks
{
    public GameObject PlayerScorePrefab;
    public Transform ScorePanel;
    Dictionary<int, GameObject> playerScores;




    // Start is called before the first frame update
    void Start()
    {
        playerScores = new Dictionary<int, GameObject>();
        foreach (var player in PhotonNetwork.PlayerList)
        {
            player.SetScore(0);
            var playerScoreObject = Instantiate(PlayerScorePrefab, ScorePanel);
            var playerScoreObjectText = playerScoreObject.GetComponent<Text>();
            playerScoreObjectText.text = $"{player.NickName}\nScore: {player.GetScore()}";

            playerScores[player.ActorNumber] = playerScoreObject;
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        var playerScoreObject = playerScores[targetPlayer.ActorNumber];
        var playerScoreObjectText = playerScoreObject.GetComponent<Text>();
        playerScoreObjectText.text = $"{targetPlayer.NickName}\nScore: {targetPlayer.GetScore()}";
    }

}
