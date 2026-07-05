using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
   public static GameLobby Instance { get; private set; }

    public event Action OnRoomCreated;
    public event Action OnRoomJoined;
    public event Action OnOpponentJoined;

    private const int MAX_NUMBER_OF_PLAYERS = 2;

    private Lobby joinedLobby;
    private float heartBeatTimer;
    private ILobbyEvents m_LobbyEvents;

    private void Awake()
    {
        Instance = this;
        InitializeUnityAuthentication();
    }

    private async void Start()
    {
        
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

    

    

    private string GenerateLobbyName()
    {
        string lobbyName = "Lobby" + UnityEngine.Random.Range(1000, 9999).ToString();
        return lobbyName;
    }

    public async void PublicRoom()
    {
        try
        {
            Debug.Log("Public Room");
            var options = new QuickJoinLobbyOptions
            {
                Player = new Unity.Services.Lobbies.Models.Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject>()
                {
                    { "displayName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, RuntimeDB.Singleton.PlayerName) }
                })
            };
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            await SubscribeToLobbyEvents();
            OnRoomJoined?.Invoke();

            Debug.Log("Joined Lobby" + joinedLobby.Id);
            //NetworkManager.Singleton.StartClient();
            RuntimeDB.Singleton.playerType = RuntimeDB.PlayerType.Client;
        }
        catch (LobbyServiceException e)
        {
            try
            {
                var options = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = new Unity.Services.Lobbies.Models.Player(AuthenticationService.Instance.PlayerId, null,new Dictionary<string, PlayerDataObject>()
                    {
                        { "displayName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, RuntimeDB.Singleton.PlayerName) }
                    })
                    
                };
                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(GenerateLobbyName(), MAX_NUMBER_OF_PLAYERS, options);
                await SubscribeToLobbyEvents();
                OnRoomCreated?.Invoke();
               // NetworkManager.Singleton.StartHost();
               RuntimeDB.Singleton.playerType = RuntimeDB.PlayerType.Host;
                Debug.Log("Joined Lobby" + joinedLobby.Id);
            }
            catch(LobbyServiceException exp)
            {
                Debug.LogError(exp.Message);
            }
            
           
            //UIManager.Instance.StartHost();
        }
    }

    public async void CreatePrivateRoom()
    {
        var options = new CreateLobbyOptions
        {
            IsPrivate = true,
            Player = new Unity.Services.Lobbies.Models.Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject>()
                    {
                        { "displayName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, RuntimeDB.Singleton.PlayerName) }
                    })

        };
        joinedLobby = await LobbyService.Instance.CreateLobbyAsync(GenerateLobbyName(), MAX_NUMBER_OF_PLAYERS, options);
        await SubscribeToLobbyEvents();
        OnRoomCreated?.Invoke();
        RuntimeDB.Singleton.playerType = RuntimeDB.PlayerType.Host;
        //NetworkManager.Singleton.StartHost();
        Debug.Log("Joined Lobby" + joinedLobby.Id);
    }

    public async void JoinPrivateRoom(string lobbyCode)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            await SubscribeToLobbyEvents();
            OnRoomCreated?.Invoke();
            //NetworkManager.Singleton.StartClient();
            RuntimeDB.Singleton.playerType = RuntimeDB.PlayerType.Client;
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }
   

    public Lobby GetLobby()
    {
        return joinedLobby;
    }

    private async Task SubscribeToLobbyEvents()
    {
        var callbacks = new LobbyEventCallbacks();
        callbacks.PlayerJoined += Lobby_PlayerJoined; ;
        

        try
        {
            m_LobbyEvents = await Lobbies.Instance.SubscribeToLobbyEventsAsync(joinedLobby.Id, callbacks);
            Debug.Log("[Lobby] Subscribed to lobby events.");
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogWarning("[Lobby] Subscribe failed: " + ex);
            // fallback: start polling GetLobbyAsync periodically
        }
    }

    private void Lobby_PlayerJoined(List<LobbyPlayerJoined> players)
    {
        Debug.Log("New player joined");
        foreach(var player in players)
        {
            if(player.Player.Id != RuntimeDB.Singleton.PlayerName)
            {

                if (player.Player.Data != null && player.Player.Data.TryGetValue("displayName", out var pdo))
                {
                    RuntimeDB.Singleton.OpponentPlayerName = pdo.Value;
                    OnOpponentJoined?.Invoke();
                }
                
            }
        }
    }
}
