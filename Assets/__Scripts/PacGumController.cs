using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacGumController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Pacgumstart");
        GameManager.instance.pacGumMaxNumber++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player");
            gameObject.SetActive(false);
            GameManager.instance.PickedUpPacGum();
        }
    }
}
