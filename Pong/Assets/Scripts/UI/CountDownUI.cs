using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CountDownUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countdownText;

    private float timer = 1;
    private int count = 3;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if(count > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = 1;
                count--;
                countdownText.text = count.ToString();
            }
        }
        else if(count == 0)
        {
            Hide();
            GameManager.Instance.SpawnGameObjects();
            GameManager.Instance.UpdateState(GameManager.State.GamePlaying);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
