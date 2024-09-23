using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_InputField createText;
    public TMP_InputField joinText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Create()
    {
        if (createText != null)
        {
            string sessionCode = createText.text;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CreateRoom(sessionCode);
            }
            else
            {
                Debug.Log("Game Manager is null");
            }
        }
    }

    public void Join()
    {
        if (joinText != null)
        {
            string sessionCode = joinText.text;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.JoinRoom(sessionCode);
            }
            else
            {
                Debug.Log("Game Manager is null");
            }
        }
    }
}
