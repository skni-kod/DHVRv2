using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Gun {
    public LineRenderer _lineRenderer;
    public Transform _lineStartPoint;
    public float _lineDisplayTime;
    public float _fireRadius;
    public float _timeBetweenBillets;

    private float _bulletTimer;

    protected override void Update(){
        base.Update();

        _bulletTimer += Time.deltaTime;
        if(_bulletTimer > _timeBetweenBillets){
            _canFire = true;
        }
    }

    public override void Fire() {
        if (_currentAmmo <= 0)
            return;

        _bulletTimer = 0;
        _canFire = false;

        StartCoroutine(HandleEffects());

        if (!infiniteAmmo) {
            _currentAmmo--;
        }

        Ray ray = new Ray(_gunTip.position, _gunTip.forward);

        if (Physics.SphereCastNonAlloc(ray, _fireRadius, _fireResults) > 0) {
            foreach (var hit in _fireResults) {
                var damageable = hit.collider?.GetComponentInParent<Damageable>();
                if (damageable) {
                    damageable.Damage(100);
                }
            }
        }
    }

    protected override IEnumerator HandleEffects() {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, _lineStartPoint.position);
        _lineRenderer.SetPosition(1, _lineStartPoint.position + _gunTip.forward * 20);

        yield return new WaitForSeconds(_lineDisplayTime);

        _lineRenderer.enabled = false;
    }
}