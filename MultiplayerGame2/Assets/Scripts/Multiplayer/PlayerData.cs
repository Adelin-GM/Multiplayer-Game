using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string id;
    public string username;
    public int beastScore;
    public DateTime date;
    public int totalPlayersInTheGame;
    public string roomName;
    public int roundsWon;
    public bool showEnemyNameTags;
    public bool showYourNameTag;

    public PlayerData()
    {
        id = Guid.NewGuid().ToString();
    }

}
