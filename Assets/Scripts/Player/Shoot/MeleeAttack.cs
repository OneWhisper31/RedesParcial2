using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MeleeAttack : NetworkBehaviour { 

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out PlayerModel otherPlayer))
        {
            otherPlayer.TakeDamage(100);
        }
    }
}
