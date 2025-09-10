using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public void OnMultiplayerClicked()
    {
        LoadingScene.Singleton.LoadScene(LoadingScene.SCENE_MULTIPLAYER);
    }

    public void OnSinglePlayerClick()
    {
        
    }

    public void OnSettingsClick()
    {

    }

    public void OnQuitClick()
    {

    }
}
