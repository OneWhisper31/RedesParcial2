using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
//[RequireComponent(typeof(LifeHandler))]
public class CharacterMovementHandler : NetworkBehaviour
{
    NetworkCharacterControllerCustom _myCharacterController;

    NetworkMecanimAnimator _animator;

    float _movementValue;

    void Awake()
    {
        _myCharacterController = GetComponent<NetworkCharacterControllerCustom>();

        //var lifeHandler = GetComponent<LifeHandler>();

        //lifeHandler.OnDeadState += SetControllerEnabled;
        //lifeHandler.OnRespawn += Respawn;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            //MOVIMIENTO

            Vector3 moveDirection = Vector3.forward * networkInputData.movementInput;

            _myCharacterController.Move(moveDirection);

            //SALTO

            if (networkInputData.isJumpPressed)
            {
                _myCharacterController.Jump();
            }

            //ANIMATOR

            _movementValue = _myCharacterController.Velocity.x;

            //_animator.Animator.SetFloat("MovementValue", _movementValue);
        }
    }

    void Respawn()
    {
        //Aquel que quiera agregar un Respawn con posiciones 'random' puede aplicar ese vector3 como parametro
        _myCharacterController.TeleportToPosition(transform.position);
    }

    void SetControllerEnabled(bool enable)
    {
        _myCharacterController.Controller.enabled = enable;
    }
}
