using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    [SerializeField] NetworkRigidbody _myNetRgbd;

    TickTimer _expireTickTimer = TickTimer.None;

    private void Start()
    {
        _myNetRgbd.Rigidbody.AddForce(transform.forward * 10, ForceMode.VelocityChange);

        if (Object.HasStateAuthority)
        {
            _expireTickTimer = TickTimer.CreateFromSeconds(Runner, 2f);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (Object && Object.HasStateAuthority)
        {
            //Si tiene el componente de vida, ejecutar su TakeDamage(25f);
            if (other.TryGetComponent(out LifeHandler lifeHandler))
            {
                lifeHandler.TakeDamage(25);
            }

            DespawnObject();
        }
    }
}
