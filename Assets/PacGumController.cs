using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacGumController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.GetComponent<PlayerController>())
        {
            this.enabled = false;
        }*/
    }

    private void OnDisable()
    {
        //TODO: Add counting sphere
    }
}
