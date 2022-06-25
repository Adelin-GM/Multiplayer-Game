using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundsLeaderboard : MonoBehaviour
{
    public int MaxResult = 5;
    public RoundsLeaderboardPopup LeaderboardPopup;
   public void SubmitScore(int roundsWon)
    {
        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate()
                {
                    StatisticName = "Rounds Won",
                    Value =roundsWon
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request,
            (result) => { Debug.Log("PlayFab - Score submitted"); },
            (error) => { Debug.Log("PlayFab - Error occurd while submitting the request: " + error.ErrorMessage); });
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest()
        {
            MaxResultsCount = MaxResult,
            StatisticName = "Rounds Won"
        };
    

        PlayFabClientAPI.GetLeaderboard(request,
            (result) => {
                Debug.Log("PlayFab - Get leaderboard completed!");
                LeaderboardPopup.UpdateUI(result.Leaderboard);
            },
            (error) => { Debug.Log("PlayFab - Error occurd while retrieving the score: : " + error.ErrorMessage); });
    }

}
