using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Gun
{
    public Rocket _rocketPrefab;

    public override void Fire()
    {
        var rocket = Instantiate(_rocketPrefab, _gunTip.position, _gunTip.rotation);
    }

    protected override IEnumerator HandleEffects()
    {
        yield return null;
    }
}
