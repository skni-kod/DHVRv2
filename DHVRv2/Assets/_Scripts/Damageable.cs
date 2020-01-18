using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour {

    public float _maxHealth = 1;

    public UnityEvent _onDeath;

    public float _currentHealth;
    protected bool _isDead;

    protected virtual void Awake() {
        _currentHealth = _maxHealth;
    }

    public virtual void Damage(float dmg) {
        if (_isDead)
            return;
        
        _currentHealth -= dmg;

        if (_currentHealth <= 0) {
            Death();
        }
    }

    public virtual void Death() {
        _isDead = true;
        
        _onDeath.Invoke();
        Destroy(gameObject);
    }
}
