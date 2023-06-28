using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifebarManager : MonoBehaviour
{
    [SerializeField] Lifebar _lifeBarPrefab;

    public event Action OnUpdateBar = delegate { };

    public void SpawnBar(PlayerModel target)
    {
        Lifebar lifebar = Instantiate(_lifeBarPrefab, target.transform.position, Quaternion.identity, transform)
                          .SetTarget(target);

        OnUpdateBar += lifebar.UpdatePosition;

        target.OnPlayerDestroyed += () => {

            OnUpdateBar -= lifebar.UpdatePosition;
            Destroy(lifebar);
        };
    }

    // Update is called once per frame
    void LateUpdate()
    {
        OnUpdateBar();
    }
}
