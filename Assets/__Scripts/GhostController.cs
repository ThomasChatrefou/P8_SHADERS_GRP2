using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _dissolveSpeed = 0.005f;
    [SerializeField] private int _multiplicationCoefficient = 4;
    [SerializeField] public GameObject _spawnPointToInstantiate;

    public GameObject GhostBody;
    private Material _GhostBodyMaterial;
    public GameObject GhostEye;
    private Material _GhostEyeMaterial;

    private float _GhostWobbleSpeedValue;

    private GameObject _player;
    private bool _EnteredTriggerWall;
    private bool _ExitedTriggerWall;
    private bool _IsGoingBackToSpawn;
    private GameObject _spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _spawnPoint = Instantiate(_spawnPointToInstantiate, transform);
        _GhostBodyMaterial = GhostBody.GetComponent<MeshRenderer>().material;
        _GhostEyeMaterial = GhostEye.GetComponent<MeshRenderer>().material;
        _GhostWobbleSpeedValue = _GhostBodyMaterial.GetFloat(Shader.PropertyToID("_WobbleSpeed"));
        _EnteredTriggerWall = false;
        _ExitedTriggerWall = false;
        _IsGoingBackToSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsGoingBackToSpawn)
        {
            if (Vector3.Distance(transform.position, _spawnPoint.transform.position) >= _minDistance)
            {
                transform.LookAt(_spawnPoint.transform);
                transform.position += transform.forward * _moveSpeed * Time.deltaTime;
            }
            else
            {
                _IsGoingBackToSpawn = false;
                _moveSpeed /= _multiplicationCoefficient;
                _GhostWobbleSpeedValue /= _multiplicationCoefficient;
                _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_WobbleSpeed"), _GhostWobbleSpeedValue);
                _GhostEyeMaterial.SetFloat(Shader.PropertyToID("_WobbleSpeed"), _GhostWobbleSpeedValue);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, _player.transform.position) >= _minDistance)
            {
                transform.LookAt(_player.transform);
                transform.position += transform.forward * _moveSpeed * Time.deltaTime;
            }
        }
        if (_EnteredTriggerWall)
        {
            if (_GhostBodyMaterial.GetFloat(Shader.PropertyToID("_Dissolve")) < 1.0f)
            {
                float valueDissolveGhostBody = _GhostBodyMaterial.GetFloat(Shader.PropertyToID("_Dissolve"));
                _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_Dissolve"), valueDissolveGhostBody + _dissolveSpeed);
            }
            else
            {
                _EnteredTriggerWall = false;
            }
        }

        if (_ExitedTriggerWall)
        {
            if (_GhostBodyMaterial.GetFloat(Shader.PropertyToID("_Dissolve")) > 0.0f)
            {
                float valueDissolveGhostBody = _GhostBodyMaterial.GetFloat(Shader.PropertyToID("_Dissolve"));
                _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_Dissolve"), valueDissolveGhostBody - _dissolveSpeed);
            }
            else
            {
                _ExitedTriggerWall = false;
                _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_StartDissolve"), 0);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_StartDissolve"), 1);
            _EnteredTriggerWall = true;
            _ExitedTriggerWall = false;
        }

        if (other.CompareTag("Player") && !_IsGoingBackToSpawn)
        {
            _IsGoingBackToSpawn = true;
            _moveSpeed *= _multiplicationCoefficient;
            _GhostWobbleSpeedValue *= _multiplicationCoefficient;
            _GhostBodyMaterial.SetFloat(Shader.PropertyToID("_WobbleSpeed"), _GhostWobbleSpeedValue);
            _GhostEyeMaterial.SetFloat(Shader.PropertyToID("_WobbleSpeed"), _GhostWobbleSpeedValue);
            other.GetComponent<PlayerBehaviour>().Damage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            _EnteredTriggerWall = false;
            _ExitedTriggerWall = true;
        }
    }
}
