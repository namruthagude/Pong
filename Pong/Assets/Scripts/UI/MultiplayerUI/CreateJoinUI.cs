using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateJoinUI : MonoBehaviour
{
   public void OnCreateButtonClicked()
    {
        GameLobby.Instance.CreatePrivateRoom();
    }

    public void OnJoinButtonClicked()
    {
        MainMenuUI.Instance.ShowJoinRoomPanel();
    }
}
