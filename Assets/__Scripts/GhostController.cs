using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _dissolveSpeed = 0.005f;

    public GameObject GhostBody;
    private Material _GhostBodyMaterial;
    public GameObject GhostEye;
    private Material _GhostEyeMaterial;

    private GameObject _player;
    private bool _EnteredTrigger;
    private bool _ExitedTrigger;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _GhostBodyMaterial = GhostBody.GetComponent<MeshRenderer>().material;
        _GhostEyeMaterial = GhostEye.GetComponent<MeshRenderer>().material;
        _EnteredTrigger = false;
        _ExitedTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_player.transform);

        if (Vector3.Distance(transform.position, _player.transform.position) >= _minDistance)
        {
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        }

        if (_EnteredTrigger)
        {
            if (_GhostBodyMaterial.GetFloat(Shader.PropertyToID("_Dissolve")) < 1.0f || _GhostEyeMaterial.GetFloat(Shader.PropertyToID("_Dissolve")) < 1.0f)
            {
                float valueDissolveGhostBody = _GhostBodyMaterial.GetFloat(Shader.PropertyToID("_Dissolve"));
                _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_Dissolve"), valueDissolveGhostBody + _dissolveSpeed);
                float valueDissolveGhostEye = _GhostEyeMaterial.GetFloat(Shader.PropertyToID("_Dissolve"));
                _GhostEyeMaterial.SetFloat(Shader.PropertyToID("_Dissolve"), valueDissolveGhostEye + _dissolveSpeed);
            }
            else
            {
                _EnteredTrigger = false;
            }
        }

        if (_ExitedTrigger)
        {
            if (_GhostBodyMaterial.GetFloat(Shader.PropertyToID("_Dissolve")) > 0.0f || _GhostEyeMaterial.GetFloat(Shader.PropertyToID("_Dissolve")) > 0.0f)
            {
                float valueDissolveGhostBody = _GhostBodyMaterial.GetFloat(Shader.PropertyToID("_Dissolve"));
                _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_Dissolve"), valueDissolveGhostBody - _dissolveSpeed);
                float valueDissolveGhostEye = _GhostEyeMaterial.GetFloat(Shader.PropertyToID("_Dissolve"));
                _GhostEyeMaterial.SetFloat(Shader.PropertyToID("_Dissolve"), valueDissolveGhostEye - _dissolveSpeed);
            }
            else
            {
                _ExitedTrigger = false;
                _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_StartDissolve"), 0);
                _GhostEyeMaterial.SetFloat(Shader.PropertyToID("_StartDissolve"), 0);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WallScript>())
        {
            _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_StartDissolve"), 1);
            _GhostEyeMaterial.SetFloat(Shader.PropertyToID("_StartDissolve"), 1);
            _EnteredTrigger = true;
            _ExitedTrigger = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<WallScript>())
        {
            _EnteredTrigger = false;
            _ExitedTrigger = true;
        }
    }
}
