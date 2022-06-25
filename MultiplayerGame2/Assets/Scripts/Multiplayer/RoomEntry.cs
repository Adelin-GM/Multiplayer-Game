using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    public Text RoomText;
    public string RoomName;
    
    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(RoomName);
    }
}
