using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(SphereCollider))]
public class WallController : MonoBehaviour
{
    private NavMeshObstacle _nmObstacle;

    private void Awake()
    {
        //_nmObstacle = GetComponent<NavMeshObstacle>();
        //SphereCollider sphCol = GetComponent<SphereCollider>();
        //
        //if (transform.position.magnitude > sphCol.radius)
        //    _nmObstacle.enabled = false;
        //else
        //    _nmObstacle.enabled = true;
    }
}