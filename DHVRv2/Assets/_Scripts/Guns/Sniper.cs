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

    protected override void Update() {
        base.Update();

        _bulletTimer += Time.deltaTime;
        if (_bulletTimer > _timeBetweenBillets) {
            _canFire = true;
        }
    }

    public override void Fire() {

        _bulletTimer = 0;
        _canFire = false;

        RaycastHit hit;
        var damageable = ScanHitSingleTarget(_fireRadius, out hit);
        if (damageable) {
            StartCoroutine(SetLineRendererForTime(_lineRenderer, _lineStartPoint.position, hit.point, _lineDisplayTime));
        } else {
            var endPoint = _lineStartPoint.position + _lineStartPoint.forward * 20f;
            StartCoroutine(SetLineRendererForTime(_lineRenderer, _lineStartPoint.position, endPoint, _lineDisplayTime));
        }
    }
}