using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] int initLife = 3;
    [SerializeField] AnimationCurve flashingSequence;

    public int life;

    private Material chompMat;

    void Awake()
    {
        chompMat = GetComponent<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        chompMat.SetFloat("_ColorMultiplier", 1);
        life = initLife;
    }

    void Update()
    {
        float evaluatedValue = flashingSequence.Evaluate(Time.time * (initLife - life));
        Debug.Log(evaluatedValue);
        chompMat.SetFloat("_ColorMultiplier", evaluatedValue);
    }

    public void Damage()
    {
        life--;
        if(life <= 0)
        {
            Debug.Log("You lost");
        }
    }
}
