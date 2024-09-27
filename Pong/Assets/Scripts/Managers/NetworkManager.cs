using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Photon;
using Fusion.Sockets;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkManager Instance;

    public Transform clientSpawnPoint;
    public Transform hostSpawnPoint;
    public NetworkObject paddlePrefab;

    public Transform ballSpawnPos;
    public NetworkObject ballPrefab;
    int playerJoined = 0;

    NetworkRunner runner;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public async void StartSession(string sessionCode)
    {
        
        if (gameObject.GetComponent<NetworkRunner>() != null)
        {
            runner = gameObject.GetComponent<NetworkRunner>();
        }
        else
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Server,
            SessionName = sessionCode, // The session code used as room name
            //Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            Debug.Log("Session created successfully with code: " + sessionCode);
        }
        else
        {
            Debug.LogError("Failed to create session: " + result.ShutdownReason);
        }
    }
    public async void JoinSession(string sessionCode)
    {
        if (gameObject.GetComponent<NetworkRunner>() != null)
        {
            runner = gameObject.GetComponent<NetworkRunner>();
        }
        else
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        Debug.Log("Session code" + sessionCode);
        Debug.Log(runner.ToString());
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = sessionCode, // The session code used as room name
           // Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            Debug.Log("Session created successfully with code: " + sessionCode);
        }
        else
        {
            Debug.LogError("Failed to create session: " + result.ShutdownReason);
        }
    }
    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        playerJoined = playerJoined + 1;
        if(runner.IsServer && player == runner.LocalPlayer)
        {
            SpawnPaddle(hostSpawnPoint.position, player);
        }
        else if(runner.IsClient)
        {
            Debug.Log("Client joined, waiting for host to sync spawn");
        }
        else
        {
            SpawnPaddle(clientSpawnPoint.position, player);

        }

        if(playerJoined == 2 && runner.IsServer)
        {
            SpawnBall(ballSpawnPos.position);
        }

    }

    void SpawnPaddle(Vector3 position, PlayerRef player)
    {
        NetworkObject  paddle = runner.Spawn(paddlePrefab, position, Quaternion.identity, player);
        paddle.AssignInputAuthority(player);
        Debug.Log("Player spawned at position " + position);
        Debug.Log($"Paddle spawned for player {player} with InputAuthority assigned.");
    }
    void SpawnBall(Vector3 position)
    {
        runner.Spawn(ballPrefab, position, Quaternion.identity);
    }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef player)
    {
        // Your code for when an object enters the player's area of interest
    }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef player)
    {
        // Your code for when an object enters the player's area of interest
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {
    
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        // Handle the reliable data received event
        Debug.Log($"Reliable data received from player {player}");
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        // Handle the progress of reliable data being received
        Debug.Log($"Reliable data progress from player {player}: {progress * 100}%");
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, System.Collections.Generic.List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
