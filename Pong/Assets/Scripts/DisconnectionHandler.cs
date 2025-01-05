using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DisconnectionHandler : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
       if(clientId == NetworkManager.ServerClientId)
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
