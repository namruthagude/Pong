using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject go_StartPanel;
    // Start is called before the first frame update
    void Start()
    {
        ShowStartPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TurnOffAllPanels()
    {
        go_StartPanel.SetActive(false);
    }

    public void ShowStartPanel()
    {
        TurnOffAllPanels();
        go_StartPanel.SetActive(true);
    }
}
