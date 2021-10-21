using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [Header("Controller Settings")]
    [SerializeField] private float _speed = 6.0f;
    [SerializeField] private float _jumpSpeed = 8.0f;
    [SerializeField] private float _gravity = 20.0f;
    private Vector3 _direction, _velocity;

    private Camera _mainCam;
    [Header("Camera Settings")]
    [SerializeField] private float _camSensitivity = 2.0f;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("Character Controller is NULL");
        }

        _mainCam = Camera.main;
        if (_mainCam == null)
        {
            Debug.LogError("Main Camera is NULL");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Movement();
        MovementView();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (_controller.isGrounded)
        {
            _direction = new Vector3(horizontal, 0f, vertical);
            _velocity = _direction * _speed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = _jumpSpeed;
            }
        }

        _velocity.y -= _gravity * Time.deltaTime;
        _velocity = transform.TransformDirection(_velocity);
        _controller.Move(_velocity * Time.deltaTime);
    }

    void MovementView()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 currentRot = transform.localEulerAngles;
        currentRot.y += mouseX * _camSensitivity;
        transform.localRotation = Quaternion.AngleAxis(currentRot.y, Vector3.up);

        Vector3 currentCamRot = _mainCam.gameObject.transform.localEulerAngles;
        currentCamRot.x -= mouseY * _camSensitivity;
        _mainCam.gameObject.transform.localRotation = Quaternion.AngleAxis(currentCamRot.x, Vector3.right);
    }
}
