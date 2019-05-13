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
    public float _damage;

    public int _maxAmmo = 3;
    public bool infiniteAmmo { get; set; }

    public Transform _gunTip;

    protected int _currentAmmo;

    protected Interactable _interactable;

    protected bool _canFire = true;

    protected virtual void Awake() {
        _interactable = GetComponent<Interactable>();
    }

    protected virtual void Start() {
        RefreshAmmo();
        infiniteAmmo = true;
    }

    protected virtual void Update() {
        if (_interactable.attachedToHand && _canFire && HaveAmmo()) {
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

    protected virtual bool HaveAmmo() {
        return _currentAmmo > 0 || infiniteAmmo;
    }

    protected Damageable ScanHitSingleTarget(float rayRadius, out RaycastHit hit) {
        Ray ray = new Ray(_gunTip.position, _gunTip.forward);
        if (Physics.SphereCast(ray, rayRadius, out hit)) {
            if (hit.collider) {
                var damageable = hit.collider.GetComponentInParent<Damageable>();
                if (damageable) {
                    damageable.Damage(_damage);
                    return damageable;
                }
            }
        }

        return null;
    }

    protected Bullet CreateBullet(Bullet bulletPrefab) {
        return Instantiate(bulletPrefab, _gunTip.position, _gunTip.rotation);
    }

    public void RefreshAmmo() {
        _currentAmmo = _maxAmmo;
    }

    protected IEnumerator SetLineRendererForTime(LineRenderer line, Vector3 lineStart, Vector3 lineEnd, float time) {
        line.enabled = true;
        line.SetPosition(0, lineStart);
        line.SetPosition(1, lineEnd);

        yield return new WaitForSeconds(time);

        line.enabled = false;
    }

}