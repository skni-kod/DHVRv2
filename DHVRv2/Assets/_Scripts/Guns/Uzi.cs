using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uzi : Gun {
    public float _fireInterval;
    public Bullet _bulletPrefab;
    public float _bulletSpeed;

    private float _fireTimer;

    protected override void Update() {
        base.Update();

        _fireTimer += Time.deltaTime;
        if (_fireTimer > _fireInterval) {
            _canFire = true;
            _fireTimer = 0.0f;
        }
    }

    public override void Fire() {
        var bullet = CreateBullet(_bulletPrefab);
        bullet.Initialize(_bulletSpeed, _damage);
    }
}