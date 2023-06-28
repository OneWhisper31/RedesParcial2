using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class MeleeAttack : NetworkBehaviour
{
    TickTimer _expireTickTimer = TickTimer.None;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            _expireTickTimer = TickTimer.CreateFromSeconds(Runner, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out PlayerModel otherPlayer))
        {
                otherPlayer.TakeDamage(Mathf.Infinity);
        }
        DespawnObject();
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (_expireTickTimer.Expired(Runner))
            {
                DespawnObject();
            }
        }
    }

    void DespawnObject()
    {
        _expireTickTimer = TickTimer.None;

        Runner.Despawn(Object);
    }
}
