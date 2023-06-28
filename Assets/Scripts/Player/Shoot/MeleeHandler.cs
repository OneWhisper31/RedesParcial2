using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MeleeHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnMeleeChanged))]
    bool IsPunching { get; set; }


    float _lastMeleeAttack;

    [SerializeField] NetworkObject _meleeAttack;

    [SerializeField] Transform MeleeAttackPosition;

    NetworkMecanimAnimator _animator;


    public override void Spawned()
    {
        //_lifeHandler = GetComponent<LifeHandler>();
        _animator = GetComponentInParent<NetworkMecanimAnimator>();
    }

    public override void FixedUpdateNetwork()
    {
        if (NetworkPlayer.Local.player.IsDead) return;

        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isMeleePressed)
            {
                Melee();
            }
        }
    }

    void Melee()
    {
        if (Time.time - _lastMeleeAttack < 1f) return;

        _lastMeleeAttack = Time.time;

        var attack = Runner.Spawn(_meleeAttack, MeleeAttackPosition.position, MeleeAttackPosition.transform.rotation);
        attack.transform.parent = MeleeAttackPosition;

        StartCoroutine(COR_Melee());
    }

    IEnumerator COR_Melee()
    {
        IsPunching = true;

        yield return new WaitForSeconds(1f);

        IsPunching = false;
    }

    static void OnMeleeChanged(Changed<MeleeHandler> changed)
    {
        bool isMeleeCurrent = changed.Behaviour.IsPunching;

        changed.LoadOld();

        bool isMeleeOld = changed.Behaviour.IsPunching;

        if (isMeleeCurrent && !isMeleeOld)
        {
            changed.Behaviour.MeleeAnim();
        }
    }

    void MeleeAnim()
    {
        _animator.Animator.SetTrigger("punched");
    }

}
