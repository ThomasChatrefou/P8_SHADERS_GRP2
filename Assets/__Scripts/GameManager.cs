using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int pacGumCollectedNumber;
    public int pacGumMaxNumber;

    private List<InputAction> enabledActions;
    private GhostController[] ghosts;
    [SerializeField] CinemachineBrain cinemachineBrain;

    [SerializeField] AudioClip winSound;

    private bool hasWon = false;

    private void Awake()
    {
        instance = this;
        pacGumMaxNumber = 0;
        pacGumCollectedNumber = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (pacGumCollectedNumber == pacGumMaxNumber && pacGumCollectedNumber > 0 && !hasWon)
        {
            Win();
        }
    }

    public void AddingMaxPacGum()
    {
        pacGumMaxNumber++;
        CallUpdateCounter();
    }

    public void PickedUpPacGum()
    {
        pacGumCollectedNumber++;
        CallUpdateCounter();
    }

    public void CallUpdateCounter()
    {
        MenuManager.instance.UpdateCounter(pacGumCollectedNumber.ToString() + " / " + pacGumMaxNumber.ToString());
    }
    private void Win()
    {
        hasWon = true;
        SoundManager.instance.playSound(winSound);
        MenuManager.instance.DisplayMessageAndReset("You won!");
    }

    public void Pause()
    {
        enabledActions = InputSystem.ListEnabledActions();
        InputSystem.DisableAllEnabledActions();
        ghosts = FindObjectsOfType<GhostController>();
        foreach (GhostController ghost in ghosts)
        {
            ghost.enabled = false;
        }
        cinemachineBrain.enabled = false;
    }

    public void Resume()
    {
        foreach (InputAction action in enabledActions)
        {
            action.Enable();
        }
        foreach (GhostController ghost in ghosts)
        {
            ghost.enabled = true;
        }
        cinemachineBrain.enabled = true;
    }
}
