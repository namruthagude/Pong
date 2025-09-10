using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NamePanelUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField NameInput;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefStrings.PLAYERNAME))
        {
            NameInput.text = GeneratePlayerName();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOKClicked()
    {
        RuntimeDB.Singleton.PlayerName = NameInput.text;
        PlayerPrefs.SetString(PlayerPrefStrings.PLAYERNAME,NameInput.text);
        gameObject.SetActive(false);
    }

    private string GeneratePlayerName()
    {
        string name = "Guest" + Random.Range(1000, 9999).ToString();
        return name;
    }
}
