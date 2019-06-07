using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    protected Rigidbody _rigidBody;
    protected float _damage;

    protected virtual void Awake() {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Initialize(float speed, float damage) {
        _rigidBody.velocity = transform.forward * speed;
        _damage = damage;
    }

    protected virtual void OnTriggerEnter(Collider other) {
        var d = other.GetComponentInParent<Damageable>();
        if (d) {
            d.Damage(_damage);
        }

        var body = other.GetComponentInParent<Rigidbody>();
        if (body) {
            var dir = other.transform.position - transform.position;
            body.AddForceAtPosition(dir.normalized * 5f, transform.position, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}