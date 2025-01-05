using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public NetworkVariable<int> HostScore = new NetworkVariable<int>();
    public NetworkVariable<int> ClientScore = new NetworkVariable<int>();

    private void Awake()
    {
        Instance = this;
    }

    public void IncrementScore(bool ishost)
    {
        if (IsServer)
        {
            if (ishost)
            {
                HostScore.Value++;
                if(HostScore.Value >= 10)
                {
                    GameManager.Instance.UpdateState(GameManager.State.GameOver);
                    return;
                }
            }
            else 
            {
                ClientScore.Value++;
                if (ClientScore.Value >= 10)
                {
                    GameManager.Instance.UpdateState(GameManager.State.GameOver);
                    return;
                }
            }
        }
    }

   
}
