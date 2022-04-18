using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [Range(1f, 50f)]
    [SerializeField] private float _rayMaxDistance = 20f;

    [SerializeField] LayerMask _groundLayer;

    [SerializeField] private Material[] _materials;

    private Camera _mainCamera;
    private NavMeshAgent _agent;

    private void Awake()
    {

        foreach (Material material in _materials)
            material.SetVector("_PlayerPosition", transform.position);
    }

    void Start()
    {
        _mainCamera = Camera.main;    
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoRepath = false;
    }

   
    void Update()
    {
        Shader.SetGlobalVector("WS_PlayerPosition", transform.position);

        foreach(Material material in _materials)
            material.SetVector("_PlayerPosition", transform.position);

        if (!Input.GetMouseButtonDown(0))
            return;
        
        Ray cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(cameraRay, out hitInfo, _rayMaxDistance, _groundLayer.value))
        {
            _agent.SetDestination(hitInfo.point);
        }
    }
}
