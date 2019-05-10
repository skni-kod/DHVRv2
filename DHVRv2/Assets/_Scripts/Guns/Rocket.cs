using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour {
    public float _explosionRadius;
    public float _explosionTime;
    public float _rocketSpeed;

    public UnityEngine.Events.UnityEvent OnExplode;

    Rigidbody _body;

    private void Awake() {
        _body = GetComponent<Rigidbody>();
    }

    void Start() {
        Invoke("Explode", _explosionTime);
        _body.velocity = transform.forward * _rocketSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        Explode();
    }

    void Explode() {
        OnExplode.Invoke();

        var colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var collider in colliders) {
            var damageable = collider.GetComponent<Damageable>();
            if (damageable) {
                damageable.Damage(1);
            }
        }

        Destroy(gameObject);
    }
}