using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance { get; private set; }

    
    [SerializeField]
    private ScoresUI scoresUI;
    
    [SerializeField]
    private CountDownUI countDownUI;
    [SerializeField]
    private GameOverUI gameOverUI;

    


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnCountingDownToStart += GameManager_OnCountingDownToStart;
        GameManager.Instance.OnGamePlaying += GameManager_OnGamePlaying;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGamePlaying(object sender, System.EventArgs e)
    {
        ShowScoreUI();
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        ShowGameOverUIClientRpc();
        ResultClientRpc();

    }

    private void GameManager_OnCountingDownToStart(object sender, System.EventArgs e)
    {
        ShowCountDownUIClientRpc();
    }

   

    public void ShowScoreUI()
    {
        ShowScoreUIClientRpc();
    }

    [ClientRpc]
    private void ShowScoreUIClientRpc()
    {
        Debug.Log("Showing Countdown");
        scoresUI.Show();
    }

    [ClientRpc]
   private void ShowCountDownUIClientRpc()
    {
        countDownUI.Show();
    }

    [ClientRpc]
   private void ShowGameOverUIClientRpc()
    {
        gameOverUI.Show();
    }

    [ClientRpc]
    private void ResultClientRpc()
    {
        if ( IsHost)
        {
            if (ScoreManager.Instance.HostScore.Value == 10)
            {
                gameOverUI.ResultText("You Win");
            }
            else if (ScoreManager.Instance.ClientScore.Value == 10)
            {
                gameOverUI.ResultText("You Loose");
            }
        }
        else if (IsClient)
        {
            if (ScoreManager.Instance.HostScore.Value == 10)
            {
                gameOverUI.ResultText("You Loose");
            }
            else if (ScoreManager.Instance.ClientScore.Value == 10)
            {
                gameOverUI.ResultText("You Win");
            }
        }

    }

 


   
}
