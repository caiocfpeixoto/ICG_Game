using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{   
    public static SoundManager instance {get; private set;}
    private AudioSource source;

    private void Awake()
    {   
       
        source = GetComponent<AudioSource>();
        // can create duplicate objects
        // DontDestroyOnLoad(gameObject);

        // keep this object even when we go to new scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }//destroy duplicate gameobjects
        else if (instance != null && instance != this)
            destroy(gameObeject);
    }
    
    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
    
}
