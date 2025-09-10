using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisconnectionHandler : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
       // GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if(SceneManager.GetActiveScene().name == LoadingScene.SCENE_GAME)
        {
            if (clientId == NetworkManager.ServerClientId)
            {
                ScoreManager.Instance.HostScore.Value = 10;
            }
            else if (NetworkManager.Singleton.IsHost)
            {
                ScoreManager.Instance.ClientScore.Value = 10;
            }
            GameManager.Instance.UpdateState(GameManager.State.GameOver);
        }
      

    }

   
}
