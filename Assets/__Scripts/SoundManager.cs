using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioSource movingSoundSpeaker;
    [SerializeField] AudioSource soundSpeaker;
    [SerializeField] AudioClip beginningSound;

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

    void Start()
    {
        if (beginningSound)
        {
            playSound(beginningSound);
            StartCoroutine(delayStartGame());
        }
    }

    IEnumerator delayStartGame()
    {
        Time.timeScale = 0;
        List<InputAction> enabledAction = InputSystem.ListEnabledActions();
        InputSystem.DisableAllEnabledActions();
        yield return new WaitForSecondsRealtime(beginningSound.length);
        Time.timeScale = 1;
        foreach(InputAction action in enabledAction)
        {
            action.Enable();
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
