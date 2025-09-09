using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject go_PlayWithFriendpanel;
    [SerializeField]
    private GameObject go_CreateJoinPanel;
    [SerializeField]
    private GameObject go_waitingRoomPanel;

    private void Start()
    {
        ShowPlayWithFriendPanel();
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
        TurnOffAllPanels();
        go_waitingRoomPanel.SetActive(true );
    }

    public void ShowCreateJoinPanel()
    {
        TurnOffAllPanels();
        go_CreateJoinPanel.SetActive(true);
    }

}
