using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLeaderboard : MonoBehaviour
{
    public int MaxResult = 5;
    public LeaderboardPopup LeaderboardPopup;
   public void SubmitScore(int playerScore)
    {
        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate()
                {
                    StatisticName = "Most Kills",
                    Value =playerScore
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
            StatisticName = "Most Kills"
        };
    

        PlayFabClientAPI.GetLeaderboard(request,
            (result) => {
                Debug.Log("PlayFab - Get leaderboard completed!");
                LeaderboardPopup.UpdateUI(result.Leaderboard);
            },
            (error) => { Debug.Log("PlayFab - Error occurd while retrieving the score: : " + error.ErrorMessage); });
    }

}
