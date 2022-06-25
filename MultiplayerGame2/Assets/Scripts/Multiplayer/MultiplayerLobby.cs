using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerLobby : MonoBehaviourPunCallbacks
{
    [Header("Login Panel")]
    public Transform LoginPanel;
    public InputField PlayerName;

    [Header("Selection Panel")]
    public Transform SelectionPanel;


    [Header("CreateRoom Panel")]
    public Transform CreateRoomPanel;
    public InputField RoomName;


    [Header("InsideRoom Panel")]
    public Transform InsideRoomPanel;
    public GameObject TextPrefab;
    public Transform InsideRoomPanelContent;
    public GameObject StartGameButton;


    [Header("ListRooms Panel")]
    public Transform ListRoomsPanel;
    public GameObject RoomEntryPrefab;
    public Transform ListRoomPanelContent;

    [Header("Chat Panel")]
    public Chat Chat;
    public Transform ChatPanel;

    Dictionary<string, RoomInfo> cachedRoomList;


    private void Start()
    {
        PlayerName.text = "Player" + Random.Range(1, 10000);
        cachedRoomList = new Dictionary<string, RoomInfo>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void UpdateLeaderboardUsername()
    {
        var request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = PlayerName.text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
        (result) => { Debug.Log("PlayFab - Score submitted 1"); },
        (error) => { Debug.Log("PlayFab - Error occurd while submitting the request: " + error.ErrorMessage); });
    }

    #region HelperMethods
    public void ActivatePanel(string panelName)
    {
        DisableAllPanels();

        if (panelName == LoginPanel.gameObject.name)
            LoginPanel.gameObject.SetActive(true);
        else if (panelName == SelectionPanel.gameObject.name)
            SelectionPanel.gameObject.SetActive(true);
        else if (panelName == CreateRoomPanel.gameObject.name)
            CreateRoomPanel.gameObject.SetActive(true);
        else if (panelName == InsideRoomPanel.gameObject.name)
            InsideRoomPanel.gameObject.SetActive(true);
        else if (panelName == ListRoomsPanel.gameObject.name)
            ListRoomsPanel.gameObject.SetActive(true);
        else if (panelName == ChatPanel.gameObject.name)
            ChatPanel.gameObject.SetActive(true);

    }

    private void DisableAllPanels()
    {
        LoginPanel.gameObject.SetActive(false);
        SelectionPanel.gameObject.SetActive(false);
        CreateRoomPanel.gameObject.SetActive(false);
        InsideRoomPanel.gameObject.SetActive(false);
        ListRoomsPanel.gameObject.SetActive(false);
        ChatPanel.gameObject.SetActive(false);
    }

    private void DeleteChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

    #region ButtonActions
    public void LoginButtonClicked()
    {
        PhotonNetwork.LocalPlayer.NickName = PlayerName.text;
        PhotonNetwork.ConnectUsingSettings();
        UpdateLeaderboardUsername();
    }

    public void DisconnectButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }

    public void createRoomClicked()
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;


        PhotonNetwork.CreateRoom(RoomName.text, roomOptions);
    }

    public void LeaveRoomClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void ListRoomsClicked() {
        PhotonNetwork.JoinLobby();
    }

    public void LeaveLobbyClicked()
    {
        PhotonNetwork.LeaveLobby();
    }

    public void JoinRnadomRoomClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void StartGameClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("Multiplayer");
    }


    #endregion

    #region ButtonCallbacks
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
        ActivatePanel(LoginPanel.name);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master server");
        ActivatePanel(SelectionPanel.gameObject.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");

        Chat.Username = PhotonNetwork.LocalPlayer.NickName;
        var authenticationValues = new Photon.Chat.AuthenticationValues(Chat.Username);
        Chat.ChatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", authenticationValues);


        ActivatePanel(InsideRoomPanel.name);

        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);

        foreach (var player in PhotonNetwork.PlayerList)
        {
            var newPlayerRoomEntry = Instantiate(TextPrefab, InsideRoomPanelContent);
            newPlayerRoomEntry.GetComponent<Text>().text = player.NickName;
            newPlayerRoomEntry.name = player.NickName;
        }
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("RoomCreationFailed");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        Chat.ChatClient.Disconnect();
        DeleteChildren(InsideRoomPanelContent);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby!");
        ActivatePanel(ListRoomsPanel.name);
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Lobby left!");
        DeleteChildren(ListRoomPanelContent);
        cachedRoomList.Clear();
        ActivatePanel(SelectionPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room upldate: " + roomList.Count);
        DeleteChildren(ListRoomPanelContent);

        UpdateCacheRoomList(roomList);


        foreach (var room in cachedRoomList)
        {
            var newRoomEntry = Instantiate(RoomEntryPrefab, ListRoomPanelContent);
            var newRoomEntryScript = newRoomEntry.GetComponent<RoomEntry>();
            newRoomEntryScript.RoomName = room.Key;
            newRoomEntryScript.RoomText.text = $"[{room.Key}] - ({room.Value.PlayerCount} / {room.Value.MaxPlayers})";
        }

    }

    private void UpdateCacheRoomList(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            //Delete from cache if room is closed
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)        
                cachedRoomList.Remove(room.Name);            
            else
                cachedRoomList[room.Name] = room;       //Add or update cache
                
            
        }
    
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player enterd room!");
        var newPlayerRoomEntry = Instantiate(TextPrefab, InsideRoomPanelContent);
        newPlayerRoomEntry.GetComponent<Text>().text = newPlayer.NickName;
        newPlayerRoomEntry.name = newPlayer.NickName;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Player left the room!");
        foreach (Transform child in InsideRoomPanelContent)
        {
            if (child.name == otherPlayer.NickName)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("On join random room failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("On join room failed: " + message);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }



    #endregion
}
