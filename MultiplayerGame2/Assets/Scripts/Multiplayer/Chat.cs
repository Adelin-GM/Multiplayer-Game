using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour, IChatClientListener
{
    [HideInInspector]
    public string Username;
    public ChatClient ChatClient;
    public InputField InputField;
    public Text ChatContent;
    #region Unity
    // Start is called before the first frame update
    void Start()
    {
        ChatClient = new ChatClient(this);
    }

    // Update is called once per frame
    void Update()
    {
        ChatClient.Service();
    }
    #endregion
    #region PUNChat
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("Chat - " + level + " - " + message);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("Chat - OnChatStateChange - " + state);
    }

    public void OnConnected()
    {
        Debug.Log("Chat - User: " + Username + " is connected");
        ChatClient.Subscribe(PhotonNetwork.CurrentRoom.Name);
    }

    public void OnDisconnected()
    {
        Debug.Log("Chat - User: " + Username + " is disconnected");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        ChatChannel curentChat;
        if(ChatClient.TryGetChannel(PhotonNetwork.CurrentRoom.Name, out curentChat))
        {
            ChatContent.text = curentChat.ToStringMessages();
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        ChatContent.text += "\n [Private message] " + sender + ": " + message;
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            if (results[i])
            {
                Debug.Log("Chat - Subscribed to " + channels[i] + " channel");
                ChatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, "just joined the channel \n To sent a private message type /username message");
            }
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
    #endregion
    
    public void SendMessage()
    { 
        if (InputField.text == "")
        {
            return;
        }
        string message = ProfanityCheck();
        if (isPrivate(message))
        {
            return;
        }

        ChatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, message);
        InputField.text = "";
    }

    private string ProfanityCheck()
    {
        string[] badWords = { "damn", "hell ", "noob" };
        string message = InputField.text;
        foreach (var item in badWords)
        {
            if (message.Contains(item))
            {
                message = message.Replace(item.ToString(), "***");
            }
        }
        return message;
    }

    public bool isPrivate(string message)
    {
        foreach (var user in PhotonNetwork.PlayerList)
        {
            if (message.Contains($"/{user.NickName}"))
            {
                message = message.Replace($"/{user.NickName}", "");
                ChatClient.SendPrivateMessage(user.NickName, message, false, false);
                InputField.text = "";
                return true;
            }
        }
        return false;
    }


}
