using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {
    public ParticleSystem m_ParticleSystem;
    public bool inheritRotation;

    public void Spawn() {
        Quaternion rot = Quaternion.identity;
        if (inheritRotation) {
            rot = transform.rotation;
        }
        var obj = Instantiate(m_ParticleSystem, transform.position, rot);
        Destroy(obj.gameObject, m_ParticleSystem.main.duration);
    }
}