using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Gun
{
    public Rocket _rocketPrefab;
    public float _rocketSpeed;

    public static System.Action<RocketLauncher> OnRocketLaunched;

    protected override void Start()
    {
        base.Start();
    }

    public override void Fire()
    {
        var rocket = Instantiate(_rocketPrefab, _gunTip.position, _gunTip.rotation) as Rocket;
        rocket.Initialize(_rocketSpeed, _damage);

        OnRocketLaunched?.Invoke(this);
        //_light.gameObject.SendMessage("RocketLaunched", transform.forward);
    }
}
