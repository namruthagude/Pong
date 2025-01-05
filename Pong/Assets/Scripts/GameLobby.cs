using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
   public static GameLobby Instance { get; private set; }

    private const int MAX_NUMBER_OF_PLAYERS = 2;

    private Lobby joinedLobby;
    private float heartBeatTimer;

    private void Awake()
    {
        Instance = this;
        InitializeUnityAuthentication();
    }

    private void Update()
    {
        HandleHeartBeat();
    }

    private void HandleHeartBeat()
    {
        if (IsLobbyHost())
        {
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer < 0)
            {
                float heartBeatTimerMax = 15f;
                heartBeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private bool IsLobbyHost()
    {
        return joinedLobby != null &&  joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }
    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions options = new InitializationOptions();
            options.SetProfile(UnityEngine.Random.Range(0,10000).ToString());
           await  UnityServices.InitializeAsync(options);
           await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
          joinedLobby =  await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_NUMBER_OF_PLAYERS, new CreateLobbyOptions()
            {
                IsPrivate = isPrivate
            });

            UIManager.Instance.StartHost();
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            UIManager.Instance.JoinClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinWithCode(string lobbycode)
    {
        try
        {
            joinedLobby =  await LobbyService.Instance.JoinLobbyByCodeAsync(lobbycode);
            UIManager.Instance.JoinClient();
        }
        catch(LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}
