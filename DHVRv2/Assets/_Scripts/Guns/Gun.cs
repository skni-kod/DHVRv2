using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;

public enum FireType {
    Single,
    Repeat,
}

[RequireComponent(typeof(Interactable))]
public abstract class Gun : MonoBehaviour {

    public TMP_Text _ammoText;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public FireType _fireType;
    public float _damage;
    public float _reloadTime;

    public int _maxAmmo = 3;
    public bool infiniteAmmo { get; set; }

    public Transform _gunTip;

    protected int _currentAmmo;

    protected Interactable _interactable;

    protected bool _canFire = true;

    protected float _timerReload;
    protected bool _needReload;

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

            if (!infiniteAmmo) {
                _currentAmmo--;
                RefreshAmmoText();
                if (_currentAmmo == 0) {
                    _needReload = true;
                }
            }

            if (_needReload) {
                _timerReload += Time.deltaTime;

                if (_timerReload > _reloadTime) {
                    RefreshAmmo();
                    _needReload = false;
                }
            }
        }
    }

    public abstract void Fire();

    protected virtual bool HaveAmmo() {
        return _currentAmmo > 0 || infiniteAmmo;
    }

    public void RefreshAmmoText() {
        _ammoText.text = string.Format("{0}/{1}", _currentAmmo, _maxAmmo);
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
        RefreshAmmoText();
    }

    protected IEnumerator SetLineRendererForTime(LineRenderer line, Vector3 lineStart, Vector3 lineEnd, float time) {
        line.enabled = true;
        line.SetPosition(0, lineStart);
        line.SetPosition(1, lineEnd);

        yield return new WaitForSeconds(time);

        line.enabled = false;
    }

}