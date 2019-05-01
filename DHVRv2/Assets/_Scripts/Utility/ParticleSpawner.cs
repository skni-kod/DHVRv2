using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour {
    public ParticleSystem m_ParticleSystem;

    public void Spawn() {
        var obj = Instantiate(m_ParticleSystem, transform.position, transform.rotation);
        Destroy(obj.gameObject, m_ParticleSystem.main.duration);
    }
}
