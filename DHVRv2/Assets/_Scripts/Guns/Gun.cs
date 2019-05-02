using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public enum FireType {
    Single,
    Repeat,
}

[RequireComponent(typeof(Interactable))]
public abstract class Gun : MonoBehaviour {
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public FireType _fireType;

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
        if (_interactable.attachedToHand && _canFire) {
            var hand = _interactable.attachedToHand.handType;

            switch (_fireType) {
                case FireType.Repeat:
                    if (grabPinchAction.GetState(hand)) {
                        Fire();
                    }
                    break;
                case FireType.Single:

                    if (grabPinchAction.GetStateDown(hand)) {
                        Fire();
                    }
                    break;
                default:
                    Debug.LogError("Not implemented Fire type: " + _fireType);
                    break;
            }
        }
    }

    public abstract void Fire();
    protected abstract IEnumerator HandleEffects();

    public void RefreshAmmo() {
        _currentAmmo = _maxAmmo;
    }
}