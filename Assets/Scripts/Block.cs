using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private int _health = 10;

    private int _currentHealth = 0;

    private void OnEnable()
    {
        _currentHealth = _health;

    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
