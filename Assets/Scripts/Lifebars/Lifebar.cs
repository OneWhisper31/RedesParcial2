using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    [SerializeField] float _yOffset;
    [SerializeField] Image _image;

    Transform _target;

    public Lifebar SetTarget(PlayerModel player)
    {
        _target = player.transform;

        //player.OnPlayerDestroyed += () => Destroy(gameObject);
        player.OnLifeUpdate += UpdateBar;
        return this;
    }

    void UpdateBar(float amount)
    {
        StopAllCoroutines();

        StartCoroutine(LerpAmount(amount));
    }

    IEnumerator LerpAmount(float amount)
    {
        float ticks = 0;

        float startAmount = _image.fillAmount;

        while (ticks <= 1)
        {

            _image.fillAmount = Mathf.Lerp(startAmount, amount, ticks / 4);

            ticks += Time.deltaTime;

            yield return null;
        }
    }

    public void UpdatePosition()
    {
        transform.position = _target.position + Vector3.up * _yOffset;
    }
}
