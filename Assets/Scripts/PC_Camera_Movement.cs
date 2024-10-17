using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PC_Camera_Movement : MonoBehaviour
{
    [SerializeField] private Transform _camTransform;
    [SerializeField] private float velocity;
    private Vector2 _input = new Vector2(0, 0);
    private float _movementX, _movementY;

    public void OnMove(InputAction.CallbackContext context) {
        _input = context.ReadValue<Vector2>().normalized;
    }

    public void Update()
    {
        _movementX = _input.x * Time.deltaTime * velocity;
        _movementY = _input.y * Time.deltaTime * velocity;

        _camTransform.position += new Vector3(_movementX, 0, _movementY);
    }
}
