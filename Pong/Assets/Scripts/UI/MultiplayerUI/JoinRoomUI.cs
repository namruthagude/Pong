using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinRoomUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField input_Code;
   
    public void OnJoinClicked()
    {
        string lobbyCode = input_Code.text;
        GameLobby.Instance.JoinPrivateRoom(lobbyCode);
    }
   
}
