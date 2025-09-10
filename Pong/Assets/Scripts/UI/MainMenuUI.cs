using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

    public static MainMenuUI Instance;
    [SerializeField]
    private GameObject go_PlayWithFriendpanel;
    [SerializeField]
    private GameObject go_CreateJoinPanel;
    [SerializeField]
    private GameObject go_waitingRoomPanel;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameLobby.Instance.OnRoomCreated += Lobby_OnRoomCreated;
        GameLobby.Instance.OnRoomJoined += Lobby_OnRoomJoined;
        ShowPlayWithFriendPanel();
    }

    private void Lobby_OnRoomJoined()
    {
       ShowWaitingRoomPanel();
    }

    private void Lobby_OnRoomCreated()
    {
        ShowWaitingRoomPanel();
    }

    private void TurnOffAllPanels()
    {
        go_CreateJoinPanel.SetActive(false);
        go_PlayWithFriendpanel.SetActive(false);
        go_waitingRoomPanel.SetActive(false);
    }

    public void ShowPlayWithFriendPanel()
    {
        TurnOffAllPanels();
        go_PlayWithFriendpanel.SetActive(true);
    }

    public void ShowWaitingRoomPanel()
    {
        Debug.Log("Showing waiting room panel");
        TurnOffAllPanels();
        go_waitingRoomPanel.SetActive(true );
    }

    public void ShowCreateJoinPanel()
    {
        TurnOffAllPanels();
        go_CreateJoinPanel.SetActive(true);
    }

}
