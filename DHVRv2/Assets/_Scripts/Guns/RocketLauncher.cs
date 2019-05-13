using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Gun
{
    public Rocket _rocketPrefab;
    public float _rocketSpeed;

    public override void Fire()
    {
        var rocket = Instantiate(_rocketPrefab, _gunTip.position, _gunTip.rotation) as Rocket;
        rocket.Initialize(_rocketSpeed, _damage);
    }
}
