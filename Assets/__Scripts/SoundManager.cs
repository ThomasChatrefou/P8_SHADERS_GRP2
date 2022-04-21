using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioSource movingSoundSpeaker;
    [SerializeField] AudioSource soundSpeaker;

    public static SoundManager instance;

    void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void playMovingSound(AudioClip clip)
    {
        if (!movingSoundSpeaker.isPlaying && clip)
        {
            movingSoundSpeaker.clip = clip;
            movingSoundSpeaker.Play();
        }
    }

    public void stopMovingSound()
    {
        movingSoundSpeaker.Stop();
    }

    public void playSound(AudioClip clip)
    {
        soundSpeaker.clip = clip;
        soundSpeaker.Play();
    }

    public void stopSound()
    {
        soundSpeaker.Stop();
    }

    void OnDestroy()
    {
        instance = null;
    }
}
