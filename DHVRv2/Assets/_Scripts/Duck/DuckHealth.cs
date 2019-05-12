using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Events;
using UnityEngine;

public class DuckHealth : Damageable {
    public GameEvent _onDuckDeath;

    public override void Death() {
        _onDuckDeath.Raise();

        base.Death();
    }
}