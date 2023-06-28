using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    float _movementInput;

    bool _isJumpPressed;
    bool _isFirePressed;
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isMeleePressed = true;
        }
    }

    public NetworkInputData GetInputs()
    {
        _inputData.movementInput = _movementInput;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isMeleePressed = _isMeleePressed;
        _isMeleePressed = false;

        return _inputData;
    }
}
