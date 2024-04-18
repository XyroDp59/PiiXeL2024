using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractHealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int _health;

    private void Awake()
    {
        _health = maxHealth;
    }

    public void AddToHealth(int health)
    {
        _health += health;
        if (_health > maxHealth) { _health = maxHealth; }
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    protected abstract void Die();
}
