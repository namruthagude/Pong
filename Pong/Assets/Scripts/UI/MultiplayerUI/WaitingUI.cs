using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using Unity.Services.Matchmaker.Models;

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
        go_OppNameText.SetActive(true);
        go_WaitingText.SetActive(false);
        if(RuntimeDB.Singleton.playerType == RuntimeDB.PlayerType.Host)
        {
            //go_StartButton.SetActive(true);

        }
        LoadingScene.Singleton.LoadScene(LoadingScene.SCENE_GAME);
       
    }

    private void OnEnable()
    {
        if (GameLobby.Instance != null)
        {
            Lobby joinedLobby = GameLobby.Instance.GetLobby();
            lobbyCodeText.text = "Lobby Code :" + joinedLobby.LobbyCode;
            playerNameText.text = RuntimeDB.Singleton.PlayerName;
            
            if(joinedLobby.Players.Count < 2)
            {

                go_WaitingText.SetActive(true);
                go_OppNameText.SetActive(false);
                go_StartButton.SetActive(false);
            }
            else
            {
                for (int i = 0; i < joinedLobby.Players.Count; i++)
                {
                    if (joinedLobby.Players[i].Id != AuthenticationService.Instance.PlayerId)
                    {
                        if (joinedLobby.Players[i].Data != null && joinedLobby.Players[i].Data.TryGetValue("displayName", out var pdo))
                        {
                            RuntimeDB.Singleton.OpponentPlayerName = pdo.Value;
                            Lobby_OnOpponentJoined();
                        }
                       
                    }
                }
                
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
        //GameManager.Instance.UpdateState(GameManager.State.WaitingToStart);
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
