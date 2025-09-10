using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene Singleton;
    public static string SCENE_MENU = "Menu";
    public static string SCENE_MULTIPLAYER = "MultiplayerMenu";
    public static string SCENE_LOADING = "Loading";
    public static string SCENE_GAME = "GameScene";

    [SerializeField]
    private GameObject holder;

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
        HideHolder();
    }
    public void LoadScene(string sceneName)
    {

        StartCoroutine(StartLoadingCoroutine(sceneName));
    }

    private IEnumerator StartLoadingCoroutine(string sceneName)
    {
        ShowHolder();
        SceneManager.LoadScene(LoadingScene.SCENE_LOADING);
        AsyncOperation asyncop = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncop.allowSceneActivation = false;
        if(asyncop.progress <= 0.9)
        {
            yield return null;
        }
        asyncop.allowSceneActivation = true;
        if (asyncop.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadScene(LoadingScene.SCENE_LOADING);
        HideHolder();
    }

    private void ShowHolder()
    {
        holder.SetActive(true);
    }
    private void HideHolder()
    {
        holder.SetActive(false);
    }
}
