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
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State state;
    [SerializeField]
    private GameObject ballPrefab;

    private int playerConnected = 0;
    private string playerName;


    private void Awake()
    {
        Instance = this;
        playerName = PlayerPrefs.GetString(PLAYERPREFS_PLAYERNAME, "PlayerName"+UnityEngine.Random.Range(100,1000).ToString());
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        state = State.None;
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        playerConnected++;

        if (playerConnected == 2 && IsServer)
        {
            SpawnBall();
            OnPlayersJoined?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);

        ball.GetComponent<NetworkObject>().Spawn();
        UpdateState(State.CountdownToStart);
    }


    public void UpdateState(State state)
    {
        this.state = state;
        ManageState();
    }

    private void ManageState()
    {
        switch (state)
        {
            case State.WaitingToStart:
                OnWaitingToStart?.Invoke(this, EventArgs.Empty);
                break;
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
        return state == State.GamePlaying;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
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
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
        }
    }
}
