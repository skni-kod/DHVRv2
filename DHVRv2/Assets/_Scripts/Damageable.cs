using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour {

    public float m_MaxHealth = 1;

    public UnityEvent m_OnDeath;

    private float m_CurrentHealth;
    private bool m_IsDead;

    private void Awake() {
        m_CurrentHealth = m_MaxHealth;
    }

    public void Damage(float dmg) {
        if(m_IsDead)
            return;
        
        m_CurrentHealth -= dmg;

        if (m_CurrentHealth <= 0) {
            Death();
        }
    }

    void Death() {
        m_IsDead = true;
        
        m_OnDeath.Invoke();
        Destroy(gameObject);
    }
}
