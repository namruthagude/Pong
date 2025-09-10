using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeDB : MonoBehaviour
{
    public static RuntimeDB Singleton;
    public string PlayerName;
    public bool IsMultiplayer;
    public string OpponentPlayerName;
    public string LobbyCode;

    [SerializeField]
    private GameObject go_NamePanel;
    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefStrings.PLAYERNAME))
        {
            go_NamePanel.SetActive(true);
        }
        else
        {
            go_NamePanel.SetActive(false);
            PlayerName = PlayerPrefs.GetString(PlayerPrefStrings.PLAYERNAME);
        }
    }



}
