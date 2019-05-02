using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uzi : Gun {
    public float _roundsPerSecond;
    public LineRenderer _lineRenderer;
    public float _fireRadius;

    private float _timeBetweenBullets;
    private float _fireTimer;

    protected override void Start() {
        base.Start();
        _timeBetweenBullets = 1 / _roundsPerSecond;
    }

    protected override void Update() {
        base.Update();

        _fireTimer += Time.deltaTime;
        if (_fireTimer > _timeBetweenBullets) {
            _canFire = true;
        }
    }

    public override void Fire() {
        if (_currentAmmo <= 0)
            return;

        _canFire = false;
        _fireTimer = 0f;

        StartCoroutine(HandleEffects());

        if (!infiniteAmmo) {
            _currentAmmo--;
        }

        Ray ray = new Ray(_gunTip.position, _gunTip.forward);

        if (Physics.SphereCastNonAlloc(ray, _fireRadius, _fireResults) > 0) {
            foreach (var hit in _fireResults) {
                var damageable = hit.collider?.GetComponentInParent<Damageable>();
                if (damageable) {
                    damageable.Damage(1);
                }
            }
        }
    }

    protected override IEnumerator HandleEffects() {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _gunTip.position);
        _lineRenderer.SetPosition(1, _gunTip.position + _gunTip.forward * 20);

        yield return new WaitForSeconds(_timeBetweenBullets / 3f);

        _lineRenderer.enabled = false;
    }
}