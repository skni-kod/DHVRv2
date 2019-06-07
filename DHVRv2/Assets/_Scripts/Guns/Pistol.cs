using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun {
    public LineRenderer _lineRenderer;
    public float _lineDisplayTime;
    public float _fireRadius;

    public override void Fire() {
        RaycastHit hit;
        var damageable = ScanHitSingleTarget(_fireRadius, out hit);
        Vector3 endPoint;
        //endPoint = _gunTip.position + _gunTip.forward * hit.distance;

        if (hit.collider != null) {
            endPoint = _gunTip.position + _gunTip.forward * hit.distance;
        } else {
            endPoint = _gunTip.position + _gunTip.forward * 20;
        }

        StartCoroutine(SetLineRendererForTime(_lineRenderer, _gunTip.position, endPoint, _lineDisplayTime, _fireRadius));

    }
}