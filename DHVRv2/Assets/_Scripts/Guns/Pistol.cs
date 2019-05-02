using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun {
    public LineRenderer _lineRenderer;
    public float _lineDisplayTime;
    public float _fireRadius;

    public override void Fire() {
        if (_currentAmmo <= 0)
            return;

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

        yield return new WaitForSeconds(_lineDisplayTime);

        _lineRenderer.enabled = false;
    }
}