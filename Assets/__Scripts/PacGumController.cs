using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacGumController : MonoBehaviour
{
    [SerializeField] AudioClip pickupSound;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Pacgumstart");
        GameManager.instance.AddingMaxPacGum();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.playSound(pickupSound);
            gameObject.SetActive(false);
            GameManager.instance.PickedUpPacGum();
        }
    }
}
