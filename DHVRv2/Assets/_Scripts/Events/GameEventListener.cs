// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoboRyanTron.Events {
    public class GameEventListener : MonoBehaviour {
        [Tooltip("Events to register with.")]
        public List<GameEvent> Events;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        private void OnEnable() {
            foreach (var e in Events) {
                e.RegisterListener(this);
            }
        }

        private void OnDisable() {
            foreach (var e in Events) {
                e.UnregisterListener(this);
            }
        }

        public void OnEventRaised() {
            Response.Invoke();
        }
    }
}