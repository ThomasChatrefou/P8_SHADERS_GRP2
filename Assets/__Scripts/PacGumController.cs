using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacGumController : MonoBehaviour
{
    [SerializeField] AudioClip pickupSound;
    [SerializeField] Material chompMat;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
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
            float percentage = GameManager.instance.pacGumCollectedNumber / (float)GameManager.instance.pacGumMaxNumber;
            if (percentage < 0.25f)
            {
                chompMat.SetFloat("_PercentPickedGum", 0.25f);
            }
            else if (percentage < 0.5f)
            {
                chompMat.SetFloat("_PercentPickedGum", 0.5f);
            }
            else if (percentage < 0.75f)
            {
                chompMat.SetFloat("_PercentPickedGum", 0.75f);
            }
            else
            {
                chompMat.SetFloat("_PercentPickedGum", 1f);
            }
        }
    }
}
