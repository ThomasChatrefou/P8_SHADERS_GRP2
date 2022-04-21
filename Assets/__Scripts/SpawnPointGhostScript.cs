using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointGhostScript : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = null;
    }

}
