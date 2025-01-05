using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ScoresUI : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hostScoreText;
    [SerializeField]
    private TextMeshProUGUI clientScoreText;

    private NetworkVariable<int> hostScore = new NetworkVariable<int>();
    private NetworkVariable<int> clientScore = new NetworkVariable<int>();
    private void Start()
    {
        ScoreManager.Instance.HostScore.OnValueChanged += UpdateHostScore;
        ScoreManager.Instance.ClientScore.OnValueChanged += UpdateClientScore;
        Hide();
    }

    private void UpdateHostScore(int oldScore, int newScore)
    {
        hostScoreText.text = newScore.ToString();
    }

    private void UpdateClientScore(int oldScore, int newScore)
    {
        clientScoreText.text = newScore.ToString();
    }
    private void OnEnable()
    {
        if (Ball.Instance != null)
        {
            Ball.Instance.OnClientScoreIncreased += Ball_OnClientScoreIncreased;
            Ball.Instance.OnHostScoreIncreased += Ball_OnHostScoreIncreased;
        }
        else
        {
            Debug.Log("Ball instance is null");
        }
    }

    private void OnDisable()
    {
        if (Ball.Instance != null)
        {
            Ball.Instance.OnClientScoreIncreased -= Ball_OnClientScoreIncreased;
            Ball.Instance.OnHostScoreIncreased -= Ball_OnHostScoreIncreased;
        }
        else
        {
            Debug.Log("Ball instance is null");
        }
    }

    private void Ball_OnHostScoreIncreased(object sender, System.EventArgs e)
    {
        hostScore.Value ++;
        ScoreManager.Instance.IncrementScore(true);
       
    }

    private void Ball_OnClientScoreIncreased(object sender, System.EventArgs e)
    {
       clientScore.Value ++;
        ScoreManager.Instance.IncrementScore(false);

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void UpdateClientScore()
    {
        clientScoreText.text = clientScore.ToString();
    }

    public void UpdateHostScore()
    {
        hostScoreText.text = hostScore.ToString();
    }

    public override  void OnDestroy()
    {
        base.OnDestroy();
        ScoreManager.Instance.HostScore.OnValueChanged -= UpdateHostScore;
        ScoreManager.Instance.ClientScore.OnValueChanged -= UpdateClientScore;
    }
}
