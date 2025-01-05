using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class GameJoiningUI : MonoBehaviour
{
    public event EventHandler OnWaitingForPlayer;
   
    private void Awake()
    {
        
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        OnWaitingForPlayer?.Invoke(this, EventArgs.Empty);
        Hide();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Hide();
    }
}
