using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    private InputMaster _controls;
    private InputAction _move;
    private CharacterController _controller;

    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private AudioClip _movingSound;

    private float _turnSmoothVelocity;


    private void Awake()
    {
        _controls = new InputMaster();
        _controls.Player.Shoot.performed += context => Shoot();
        _move = _controls.Player.Movement;
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Shader.SetGlobalVector("WS_PlayerPosition", transform.position);

        Vector2 inputDirection = _move.ReadValue<Vector2>();

        if (inputDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDirection.normalized * _speed * Time.deltaTime);

            SoundManager.instance.playMovingSound(_movingSound);
        }
        else
        {
            SoundManager.instance.stopMovingSound();
        }

        if (transform.position.y > 1.01f)
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }

    private void Shoot()
    {
        print("shoot");
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
