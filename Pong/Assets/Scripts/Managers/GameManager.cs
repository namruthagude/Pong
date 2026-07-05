using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class GameManager : NetworkBehaviour
{
    private const string PLAYERPREFS_PLAYERNAME = "PlayerName" ;

    public static GameManager Instance { get; private set; }

    public event EventHandler OnWaitingToStart;
    public event EventHandler OnCountingDownToStart;
    public event EventHandler OnGamePlaying;
    public event EventHandler OnGameOver;
    public event EventHandler OnPlayersJoined;

    public enum State
    {
        None,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    public NetworkVariable< State> state = new NetworkVariable<State>();
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private GameObject hostPaddlePrefab;
    [SerializeField]
    private GameObject clientPaddlePrefab;
    private int playerConnected = 0;
    private string playerName;


    private void Awake()
    {
        Instance = this;
        playerName = RuntimeDB.Singleton.PlayerName;
        
    }

    private void Start()
    {
        //state.Value = State.None;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        //state = State.None;
        if (RuntimeDB.Singleton.playerType == RuntimeDB.PlayerType.Host)
        {
            NetworkManager.Singleton.StartHost();

        }
        else if (RuntimeDB.Singleton.playerType == RuntimeDB.PlayerType.Client)
        {
            NetworkManager.Singleton.StartClient();
        }
        // NetworkManager_OnClientConnectedCallback();
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        UnityEngine.Debug.Log(" Client Connected callback");
        // Important: only the server (or host) should perform Spawn() calls
        //UpdateState(State.CountdownToStart);
        if (!NetworkManager.Singleton.IsServer) return;
        UpdateState(State.CountdownToStart);
        
        
    }

    public void SpawnGameObjects()
    {
        SpawnPlayers();
        OnPlayersJoined?.Invoke(this, EventArgs.Empty);
        SpawnBall();
    }
    private void SpawnPlayers()
    {
        // Choose prefab: if the clientId equals the server's local client id (host),
        // we spawn the host paddle for that client; otherwise spawn client paddle.
        UnityEngine.Debug.Log("Spawning Palyers");
        Debug.Log("NetworkManager.Singleton.ConnectedClientsList length" + NetworkManager.Singleton.ConnectedClientsList.Count);
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if(client.ClientId == NetworkManager.Singleton.LocalClientId)
            {
                //Host
                var player = Instantiate(hostPaddlePrefab, Vector3.zero, Quaternion.identity);
                var no = player.GetComponent<NetworkObject>();
                no.SpawnAsPlayerObject(client.ClientId);
        
            }
            else
            {
                //Client 
                var player = Instantiate(clientPaddlePrefab, Vector3.zero, Quaternion.identity);
                var no = player.GetComponent<NetworkObject>();
                no.SpawnAsPlayerObject(client.ClientId);
            }
        }
    }

    private void SpawnBall()
    {

        Debug.Log("Spawing Ball");
        // Ball should only be spawned by server too
        if (!NetworkManager.Singleton.IsServer) return;

        var ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        var no = ball.GetComponent<NetworkObject>();
        if (no == null)
        {
            Debug.LogError("Ball prefab missing NetworkObject component!");
            Destroy(ball);
            return;
        }

        // Server spawns a shared ball (no ownership needed)
        no.Spawn();

       // UpdateState(State.CountdownToStart);
    }



    public void UpdateState(State state)
    {
        this.state.Value = state;
        ManageState();
    }

    private void ManageState()
    {
        switch (state.Value)
        {
            case State.CountdownToStart:
                OnCountingDownToStart?.Invoke(this, EventArgs.Empty);
                break;
            case State.GamePlaying:
                OnGamePlaying?.Invoke(this, EventArgs.Empty);
                break;
            case State.GameOver:
                OnGameOver?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public string GetPlayerName()
    {
        return playerName;
    }
    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
        PlayerPrefs.SetString(PLAYERPREFS_PLAYERNAME, playerName);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
       
    }
}
