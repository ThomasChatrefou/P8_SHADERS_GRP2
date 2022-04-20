using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] int initLife = 3;
    [SerializeField] AnimationCurve flashingSequence;
    [SerializeField] Material chompMat;

    private int life;

    // Start is called before the first frame update
    void Start()
    {
        chompMat.SetFloat("_ColorMultiplier", 1);
        life = initLife;
    }

    void Update()
    {
        float evaluatedValue = flashingSequence.Evaluate(Time.time * (initLife - life));
        chompMat.SetFloat("_ColorMultiplier", evaluatedValue);
    }

    public void Damage()
    {
        life--;
        if(life <= 0)
        {
            MenuManager.instance.DisplayMessageAndReset("You lost");
        }
    }
}
