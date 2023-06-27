using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LifeHandler : NetworkBehaviour
{
    const byte FULL_LIFE = 100;

    [Networked(OnChanged = nameof(OnLifeChanged))]
    byte _currentLife { get; set; }

    [Networked(OnChanged = nameof(OnStateChanged))]
    bool _isDead { get; set; }

    public bool IsDead => _isDead;

    [SerializeField] GameObject _visualObject;

    public event System.Action<bool> OnDeadState = delegate { };
    public event System.Action OnRespawn = delegate { };

    int _currentDead = 3;

    void Start()
    {
        _currentLife = FULL_LIFE;
    }

    public void TakeDamage(byte dmg)
    {
        if (_isDead) return;

        if (dmg > _currentLife)
            dmg = _currentLife;

        _currentLife -= dmg;

        if (_currentLife == 0)
        {
            if (_currentDead > 0)
            {
                StartCoroutine(CO_Respawn());

                _isDead = true;

                _currentDead--;
            }
            else
            {
                Runner.Despawn(Object);
                
                if (!Object.HasInputAuthority)
                {
                    Runner.Disconnect(Object.InputAuthority);
                }
            }
            
        }

    }

    IEnumerator CO_Respawn()
    {
        yield return new WaitForSeconds(2f);

        Respawn();
    }

    void Respawn()
    {
        OnRespawn();

        _currentLife = FULL_LIFE;

        _isDead = false;
    }

    static void OnStateChanged(Changed<LifeHandler> changed)
    {
        bool isDeadCurrent = changed.Behaviour.IsDead;

        changed.LoadOld();

        bool isDeadOld = changed.Behaviour.IsDead;

        if (isDeadCurrent)
        {
            changed.Behaviour.Death();
        }
        else if (!isDeadCurrent && isDeadOld)
        {
            changed.Behaviour.Revive();
        }
    }

    void Death()
    {
        _visualObject.SetActive(false);

        OnDeadState(false);
    }

    void Revive()
    {
        _visualObject.SetActive(true);

        OnDeadState(true);
    }

    static void OnLifeChanged(Changed<LifeHandler> changed)
    {
        //Actualizar barra de vida
    }

}
