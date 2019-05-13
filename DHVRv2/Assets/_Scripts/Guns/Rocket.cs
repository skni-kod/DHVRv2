using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet {
    public float _explosionRadius;
    public float _explosionTime;

    public UnityEngine.Events.UnityEvent OnExplode;


    void Start() {
        Invoke("Explode", _explosionTime);
    }

    protected override void OnTriggerEnter(Collider other) {
        Explode();
    }

    void Explode() {
        OnExplode.Invoke();

        var colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var collider in colliders) {
            var damageable = collider.GetComponentInParent<Damageable>();
            if (damageable) {
                damageable.Damage(_damage);
            }
        }

        Destroy(gameObject);
    }
}