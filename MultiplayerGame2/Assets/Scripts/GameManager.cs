using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData playerData;
    public string FilePath;
    public GlobalLeaderboard GlobalLeaderboard;
    public RoundsLeaderboard RoundsLeaderboard;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        LoadPlayerData();
        LoginToPlayFab();
    }

    private void LoginToPlayFab()
    {
        var request = new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = playerData.id,
        };
        PlayFabClientAPI.LoginWithCustomID(request, PlayfabLoginResult, PlayfabLoginError);
    }

    private void PlayfabLoginResult(LoginResult loginResult)
    {
        Debug.Log("PlayFab - Logged in" + loginResult.ToJson());
    }

    private void PlayfabLoginError(PlayFabError playFabError)
    {
        Debug.Log("PlayFab - Login flase" + playFabError.ErrorMessage);
    }

    public void SavePlayerData()
    {
        var serializedData = JsonConvert.SerializeObject(playerData);
        File.WriteAllText(FilePath, serializedData);
    }

    public void LoadPlayerData()
    {
        if (!File.Exists(FilePath))
        {
            playerData = new PlayerData();
            SavePlayerData();
        }
        else
        {
        }
        
        var fileContents = File.ReadAllText(FilePath);
        playerData = JsonConvert.DeserializeObject<PlayerData>(fileContents);
    }
}
