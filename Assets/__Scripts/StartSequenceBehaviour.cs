using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSequenceBehaviour : MonoBehaviour
{

    [SerializeField] AnimationCurve cameraDistance;
    [SerializeField] AnimationCurve cameraColatitude;
    [SerializeField] AnimationCurve cameraLongitude;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera startSequenceCamera;

    [SerializeField] AnimationCurve wallsDistThresholdDown;
    [SerializeField] AnimationCurve wallsDistThresholdUp;
    [SerializeField] Material wallsMaterial;

    [SerializeField] AudioClip startSequenceAudio;

    private float startTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.gameObject.SetActive(false);
        startSequenceCamera.gameObject.SetActive(true);
        SoundManager.instance.playSound(startSequenceAudio);
        GameManager.instance.Pause();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Time.time - startTime) / startSequenceAudio.length;
        Vector3 cameraPos = startSequenceCamera.gameObject.transform.localPosition;
        cameraPos.y = cameraDistance.Evaluate(t);
        startSequenceCamera.gameObject.transform.localPosition = cameraPos;
        transform.rotation = Quaternion.AngleAxis(cameraColatitude.Evaluate(t), Vector3.up) * Quaternion.AngleAxis(cameraLongitude.Evaluate(t), Vector3.right);
        wallsMaterial.SetFloat("_DistThresholdDown", wallsDistThresholdDown.Evaluate(t));
        wallsMaterial.SetFloat("_DistThresholdUp", wallsDistThresholdUp.Evaluate(t));
        if (Time.time - startTime >= startSequenceAudio.length)
        {
            startSequenceCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            Destroy(startSequenceCamera.gameObject);
            Destroy(gameObject);
            GameManager.instance.Resume();
        }
    }
}
