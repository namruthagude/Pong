using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static  GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom(string sessionCode)
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.StartSession(sessionCode);
        }
        else
        {
            Debug.Log("Network manager is null");
        }
    }

    public void JoinRoom(string sessionCode)
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.JoinSession(sessionCode);
        }
        else
        {
            Debug.Log("Network manager is null");
        }
    }
}
