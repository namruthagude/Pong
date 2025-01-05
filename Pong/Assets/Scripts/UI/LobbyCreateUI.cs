using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button publicLobbyButton;
    [SerializeField]
    private Button privateLobbyButton;
    [SerializeField]
    private TMP_InputField lobbyNameText;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        publicLobbyButton.onClick.AddListener(() =>
        {
            GameLobby.Instance.CreateLobby(lobbyNameText.text, false);
            Hide();
        });

        privateLobbyButton.onClick.AddListener(() =>
        {
            GameLobby.Instance.CreateLobby(lobbyNameText.text, true);
            Hide();
        });
    }

    private void Start()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
