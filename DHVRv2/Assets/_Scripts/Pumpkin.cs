using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pumpkin : Damageable
{

    public static System.Action<Pumpkin> OnPumpkinDestroyed;
    public static System.Action<Pumpkin> OnPumpkinCreated;

    // Start is called before the first frame update
    void Start()
    {
        OnPumpkinCreated?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        _isDead = false;
        _currentHealth = _maxHealth;
        gameObject.SetActive(true);
    }

    public override void Death()
    {
        base.Death();
        OnPumpkinDestroyed?.Invoke(this);
    }
}
