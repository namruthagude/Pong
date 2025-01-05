using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Lobbies.Models;

public class WaitingUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text lobbyNameText;
    [SerializeField]
    private TMP_Text lobbyCodeText;


    private void Start()
    {
       
        UIManager.Instance.GetGameJoiningUI().OnWaitingForPlayer += GameJoiningUI_OnWaitingForPlayer;
        GameManager.Instance.OnPlayersJoined += BallSpawner_OnPlayersJoined;
        Hide();
    }

    private void OnEnable()
    {
        if (GameLobby.Instance != null)
        {
            Lobby joinedLobby = GameLobby.Instance.GetLobby();
            lobbyNameText.text = "Lobby Name :" + joinedLobby.Name;
            lobbyCodeText.text = "Lobby Code :" + joinedLobby.LobbyCode;
        }
    }

    private void BallSpawner_OnPlayersJoined(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameJoiningUI_OnWaitingForPlayer(object sender, System.EventArgs e)
    {
        Show();
        GameManager.Instance.UpdateState(GameManager.State.WaitingToStart);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
