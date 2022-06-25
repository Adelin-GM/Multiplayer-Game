using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardPopup : MonoBehaviour
{
    public GameObject ScoreHolder;
    public GameObject NoScoreText;
    public GameObject LeaderboardItem;

    private void OnEnable()
    {
        GameManager.Instance.GlobalLeaderboard.GetLeaderboard();
    }

    public void UpdateUI(List<PlayerLeaderboardEntry> scoreList)
    {
        if (scoreList.Count > 0)
        {
            DeleteChildren(ScoreHolder.transform);
            for (int i = 0; i < scoreList.Count; i++)
            {
                var newLeaderboardItem = Instantiate(LeaderboardItem, Vector3.zero, Quaternion.identity, ScoreHolder.transform);
                newLeaderboardItem.GetComponent<LeaderboardItem>().SetValues(i + 1, scoreList[i].DisplayName, scoreList[i].StatValue);
            }

            ScoreHolder.SetActive(true);
            NoScoreText.SetActive(false);
        }
        else
        {
            ScoreHolder.SetActive(false);
            NoScoreText.SetActive(true);
        }
    }

    private void DeleteChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
