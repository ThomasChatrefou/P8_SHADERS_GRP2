using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int pacGumCollectedNumber;
    public int pacGumMaxNumber;


    private void Awake()
    {
        instance = this;
        pacGumMaxNumber = 0;
        pacGumCollectedNumber = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        MenuManager.instance.UpdateCounter(pacGumCollectedNumber.ToString() + " / " + pacGumMaxNumber.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (pacGumCollectedNumber == pacGumMaxNumber && pacGumCollectedNumber > 0)
        {
            Win();
        }
    }

    public void PickedUpPacGum()
    {
        pacGumCollectedNumber++;
        MenuManager.instance.UpdateCounter(pacGumCollectedNumber.ToString() + " / " + pacGumMaxNumber.ToString());
    }
    private void Win()
    {
        Debug.Log("Win!");
        //TODO: Win
    }
}
