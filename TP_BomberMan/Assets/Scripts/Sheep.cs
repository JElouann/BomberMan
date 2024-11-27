using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Sheep : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    private float _health = 10;

    public void Damage()
    {
        _health--;
        _healthBar.DOFillAmount(_health / 10, 0.3f).SetEase(Ease.InOutCubic);
        if (_health <= 0) Death();
    }

    public void Death()
    {
        Destroy(gameObject);
        Destroy(_healthBar.gameObject);
    }
}
