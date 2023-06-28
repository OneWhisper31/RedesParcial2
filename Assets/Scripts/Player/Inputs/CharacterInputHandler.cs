using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    float _movementInput;

    bool _isJumpPressed;
    bool _isFirePressed;
    bool _isProtectPressed;
    bool _isMeleePressed;
    NetworkInputData _inputData;

    void Start()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        _movementInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W))
        {
            _isJumpPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _isFirePressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _isMeleePressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _isProtectPressed = true;
        }
    }

    public NetworkInputData GetInputs()
    {
        _inputData.movementInput = _movementInput;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isProtectPressed = _isProtectPressed;
        _isProtectPressed = false;

        _inputData.isMeleePressed = _isMeleePressed;
        _isMeleePressed = false;

        return _inputData;
    }
}
