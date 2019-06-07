using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TMP_Text _ammoText;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public LayerMask _hittableMask;
    public FireType _fireType;
    public float _damage;
    public float _reloadTime;

    public int _maxAmmo = 3;
    public bool infiniteAmmo { get; set; }

    public Transform _gunTip;
    public LineRenderer _guideLine;

    protected int _currentAmmo;

    protected Interactable _interactable;

    protected bool _canFire = true;

    protected float _timerReload;
    protected bool _needReload;

    private float _hitRadius;

    protected virtual void Awake() {
        _interactable = GetComponent<Interactable>();
    }

    protected virtual void Start() {
        RefreshAmmo();
        infiniteAmmo = false;
    }

    protected virtual void Update() {
        if (_interactable.attachedToHand && _canFire && HaveAmmo()) {
            var hand = _interactable.attachedToHand.handType;

            switch (_fireType) {
                case FireType.Repeat:
                    if (grabPinchAction.GetState(hand)) {
                        Fire();

                        if (!infiniteAmmo) {
                            _currentAmmo--;
                            RefreshAmmoText();
                            if (_currentAmmo == 0) {
                                _needReload = true;
                            }
                        }
                    }
                    break;
                case FireType.Single:

                    if (grabPinchAction.GetStateDown(hand)) {
                        Fire();

                        if (!infiniteAmmo) {
                            _currentAmmo--;
                            RefreshAmmoText();
                            if (_currentAmmo == 0) {
                                _needReload = true;
                            }
                        }
                    }
                    break;
                default:
                    Debug.LogError("Not implemented Fire type: " + _fireType);
                    break;
            }
        }

        if (_needReload) {
            _timerReload += Time.deltaTime;

            _ammoText.text = "Reloading...";

            if (_timerReload > _reloadTime) {
                RefreshAmmo();
                _needReload = false;

                _timerReload = 0f;
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
        _hitRadius = rayRadius;

        if (Physics.SphereCast(ray, rayRadius, out hit, float.PositiveInfinity, _hittableMask)) {
            if (hit.collider) {
                var damageable = hit.collider.GetComponentInParent<Damageable>();
                if (damageable) {
                    damageable.Damage(_damage);
                    return damageable;
                }

                var body = hit.collider.GetComponentInParent<Rigidbody>();
                if (body) {
                    body.AddForceAtPosition(ray.direction * 5f, hit.point, ForceMode.Impulse);
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

    protected IEnumerator SetLineRendererForTime(LineRenderer line, Vector3 lineStart, Vector3 lineEnd, float time, float? radius = null) {
        line.enabled = true;
        line.SetPosition(0, lineStart);
        line.SetPosition(1, lineEnd);

        if (radius.HasValue) {
            line.startWidth = radius.Value;
            line.endWidth = radius.Value;
        }   

        yield return new WaitForSeconds(time);

        line.enabled = false;
    }

    public void SetGuideLineActive(bool active) {
        _guideLine.gameObject.SetActive(active);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_gunTip.position, _hitRadius);
    }

}