using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Lobbies.Models;

public class WaitingUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerNameText;
    [SerializeField]
    private TMP_Text lobbyCodeText;
    [SerializeField]
    private TMP_Text opponentNameText;
    [SerializeField]
    private GameObject go_StartButton;
    [SerializeField]
    private GameObject go_WaitingText;
    [SerializeField]
    private GameObject go_OppNameText;
    


    private void Start()
    {

        //UIManager.Instance.GetGameJoiningUI().OnWaitingForPlayer += GameJoiningUI_OnWaitingForPlayer;
        //GameManager.Instance.OnPlayersJoined += BallSpawner_OnPlayersJoined;
        //Hide();
        GameLobby.Instance.OnOpponentJoined += Lobby_OnOpponentJoined;
    }

    private void Lobby_OnOpponentJoined()
    {
        opponentNameText.text = RuntimeDB.Singleton.OpponentPlayerName;
        go_StartButton.SetActive(true);
    }

    private void OnEnable()
    {
        if (GameLobby.Instance != null)
        {
            Lobby joinedLobby = GameLobby.Instance.GetLobby();
            lobbyCodeText.text = "Lobby Code :" + joinedLobby.LobbyCode;
            playerNameText.text = RuntimeDB.Singleton.PlayerName;
            if(RuntimeDB.Singleton.OpponentPlayerName == " " || RuntimeDB.Singleton.OpponentPlayerName == null)
            {
                go_WaitingText.SetActive(true);
                go_OppNameText.SetActive(false);
            }
            else
            {
                go_WaitingText.SetActive(false);
                go_OppNameText.SetActive(true);
                opponentNameText.text = RuntimeDB.Singleton.OpponentPlayerName;
            }
            
            if(joinedLobby.Players.Count < 2)
            {
                go_StartButton.SetActive(false);
            }
            else
            {

                go_StartButton.SetActive(true);
            }
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

    public void OnStartButtonClicked()
    {
        LoadingScene.Singleton.LoadScene(LoadingScene.SCENE_GAME);
    }
}
