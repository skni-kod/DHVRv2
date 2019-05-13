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
            Debug.Log(d.name);
            d.Damage(_damage);
        }

        Destroy(gameObject);
    }
}