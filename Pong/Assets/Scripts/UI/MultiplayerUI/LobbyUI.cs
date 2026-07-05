using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private Button mainMenuButton;
    [SerializeField]
    private Button createLobbyButton;
    [SerializeField]
    private Button quickJoinLobbyButton;

    [SerializeField]
    private Button joinByCodeButton;
    [SerializeField]
    private TMP_InputField lobbyCodeInput;
    [SerializeField]
    private TMP_InputField playerNameInput;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {

        });


        

    }

    private void Start()
    {
        playerNameInput.text = GameManager.Instance.GetPlayerName();
        playerNameInput.onValueChanged.AddListener((string newText) =>
        {
            GameManager.Instance.SetPlayerName(newText);
        });
    }

    
}
