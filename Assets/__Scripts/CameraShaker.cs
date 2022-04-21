using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _intensity;
    [SerializeField] private float _shakeTime;

    private CinemachineFreeLook _cinemachineFreeLook;
    private float _shakeTimer;

    void Awake()
    {
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    public void Shake()
    {
        SetNoiseAmplitude(_intensity);
        _shakeTimer = _shakeTime;
    }

    private void SetNoiseAmplitude(float amplitude)
    {
        CinemachineVirtualCamera virtualCam = _cinemachineFreeLook.GetRig(1);
        CinemachineBasicMultiChannelPerlin cinemachineBMCP = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBMCP.m_AmplitudeGain = amplitude;
    }

    void Update()
    {
        if(_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;

            if(_shakeTimer <= 0f)
                SetNoiseAmplitude(0f);
        }
    }
}
