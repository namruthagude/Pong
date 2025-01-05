using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI resultText;
    private void Start()
    {
        Hide();
    }

    public void ResultText(string result)
    {
        resultText.text = result;
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
