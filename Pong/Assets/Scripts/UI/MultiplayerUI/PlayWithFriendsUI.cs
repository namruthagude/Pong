using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWithFriendsUI : MonoBehaviour
{
   public void OnPublicRoomClicked()
    {
        GameLobby.Instance.PublicRoom();
    }

    public void OnPrivateRoomClicked()
    {
        MainMenuUI.Instance.ShowCreateJoinPanel();
    }
}
