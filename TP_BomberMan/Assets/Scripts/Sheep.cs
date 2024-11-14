using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    [SerializeField] private int _health;

    public void Damage()
    {
        _health--;
        print("damage");
        if (_health <= 0) Death();
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
