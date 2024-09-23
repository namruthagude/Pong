using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Photon;
using Fusion.Sockets;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public NetworkRunner runner;

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
        runner = gameObject.AddComponent<NetworkRunner>();

        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
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
        runner = gameObject.AddComponent<NetworkRunner>();

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
