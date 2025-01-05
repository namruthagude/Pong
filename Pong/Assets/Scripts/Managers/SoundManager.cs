using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip ballhitAudio;

    private Ball instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(instance == null)
        {
            if (Ball.Instance != null)
            {
                instance = Ball.Instance;
                Ball.Instance.OnCollided += Ball_OnCollided;
            }
        }
    }

    private void Ball_OnCollided(object sender, System.EventArgs e)
    {
        PlayAudioClip();
    }

    private void PlayAudioClip()
    {
        audioSource.PlayOneShot(ballhitAudio, 1);
    }
}
