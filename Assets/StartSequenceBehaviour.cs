using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private List<InputAction> enabledActions;
    private GhostController[] ghosts;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.gameObject.SetActive(false);
        startSequenceCamera.gameObject.SetActive(true);
        SoundManager.instance.playSound(startSequenceAudio);
        //Time.timeScale = 0;
        enabledActions = InputSystem.ListEnabledActions();
        InputSystem.DisableAllEnabledActions();
        wallsMaterial.SetFloat("DistThresholdDown", 50);
        wallsMaterial.SetFloat("DistThresholdUp", 50);
        ghosts = FindObjectsOfType<GhostController>();
        foreach(GhostController ghost in ghosts)
        {
            ghost.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time / startSequenceAudio.length;
        Vector3 cameraPos = startSequenceCamera.gameObject.transform.localPosition;
        cameraPos.y = cameraDistance.Evaluate(t);
        startSequenceCamera.gameObject.transform.localPosition = cameraPos;
        transform.rotation = Quaternion.AngleAxis(cameraColatitude.Evaluate(t), Vector3.up) * Quaternion.AngleAxis(cameraLongitude.Evaluate(t), Vector3.right);
        wallsMaterial.SetFloat("_DistThresholdDown", wallsDistThresholdDown.Evaluate(t));
        wallsMaterial.SetFloat("_DistThresholdUp", wallsDistThresholdUp.Evaluate(t));
        if (Time.time >= startSequenceAudio.length)
        {
            startSequenceCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            Destroy(startSequenceCamera.gameObject);
            Destroy(gameObject);
            //Time.timeScale = 1;
            foreach (InputAction action in enabledActions)
            {
                action.Enable();
            }
            foreach (GhostController ghost in ghosts)
            {
                ghost.enabled = true;
            }
        }
    }
}
