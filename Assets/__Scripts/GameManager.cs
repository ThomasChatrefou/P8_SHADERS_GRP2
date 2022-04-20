using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int pacGumCollectedNumber;
    public int pacGumMaxNumber;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            pacGumMaxNumber = 0;
            pacGumCollectedNumber = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pacGumCollectedNumber == pacGumMaxNumber && pacGumCollectedNumber > 0)
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("Win!");
        //TODO: Win
    }
}
