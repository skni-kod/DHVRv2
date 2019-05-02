using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour {

    public float _maxHealth = 1;

    public UnityEvent _onDeath;

    private float _currentHealth;
    private bool _isDead;

    private void Awake() {
        _currentHealth = _maxHealth;
    }

    public void Damage(float dmg) {
        if(_isDead)
            return;
        
        _currentHealth -= dmg;

        if (_currentHealth <= 0) {
            Death();
        }
    }

    void Death() {
        _isDead = true;
        
        _onDeath.Invoke();
        Destroy(gameObject);
    }
}
