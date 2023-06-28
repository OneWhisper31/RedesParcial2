using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


//[RequireComponent(typeof(LifeHandler))]
public class WeaponHandler :  NetworkBehaviour
{
    /*[SerializeField] Bullet _bulletPrefab;
    [SerializeField] Transform _firingTransform;
    [SerializeField] ParticleSystem _shootParticle;

    [Networked(OnChanged = nameof(OnFireChange))]
    bool IsFiring { get; set; }

    float _lastFireTime;

    LifeHandler _lifeHandler;

    private void Awake()
    {
        _lifeHandler = GetComponent<LifeHandler>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_lifeHandler.IsDead) return;

        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFirePressed)
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        if (Time.time - _lastFireTime < 0.15f) return;

        _lastFireTime = Time.time;

        var newBullet = Runner.Spawn(_bulletPrefab, _firingTransform.position, transform.rotation);

        //Si se quiere usar raycast como en un FPS:

        /*Runner.LagCompensation.Raycast(origin: _firingTransform.position,
                                       direction: _firingTransform.forward, 
                                       length: 100, 
                                       player: Object.InputAuthority, 
                                       hit: out var hitInfo);

        if (hitInfo.Hitbox != null)
        {
            if (Object.HasStateAuthority)
            {
                hitInfo.Hitbox.transform.root.GetComponent<LifeHandler>()?.TakeDamage(25);
            }
        }

        StartCoroutine(COR_Fire());
    }
    
    IEnumerator COR_Fire()
    {
        IsFiring = true;

        yield return new WaitForSeconds(0.15f);

        IsFiring = false;
    }

    static void OnFireChange(Changed<WeaponHandler> changed)
    {
        bool isFiringCurrent = changed.Behaviour.IsFiring;

        changed.LoadOld();

        bool isFiringOld = changed.Behaviour.IsFiring;

        if (isFiringCurrent && !isFiringOld)
        {
            changed.Behaviour.FireRemote();
        }
    }

    void FireRemote()
    {
        _shootParticle.Play();
    }*/
}
