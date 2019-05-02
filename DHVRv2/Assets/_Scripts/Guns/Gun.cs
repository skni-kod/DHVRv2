using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public abstract class Gun : MonoBehaviour {
    public int _maxAmmo = 3;
    public int _maxTargetHitCount = 5;
    public bool infiniteAmmo { get; set; }

    public Transform _gunTip;
   
    protected int _currentAmmo;

    protected Interactable _interactable;

    protected RaycastHit[] _fireResults;

    protected bool _canFire = true;

    protected virtual void Awake() {
        _fireResults = new RaycastHit[_maxTargetHitCount];
        _interactable = GetComponent<Interactable>();

    }

    protected virtual void Start() {
        RefreshAmmo();
        infiniteAmmo = true;
    }

    protected virtual void Update() {
        if (_interactable.attachedToHand) {
            var hand = _interactable.attachedToHand.handType;
            if (SteamVR_Input.GetStateDown("default", "GrabPinch", hand) && _canFire) {
                Fire();
            }
        }
    }

    public abstract void Fire();
    protected abstract IEnumerator HandleEffects();

    public void RefreshAmmo() {
        _currentAmmo = _maxAmmo;
    }
}