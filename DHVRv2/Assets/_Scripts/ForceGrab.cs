using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ForceGrab : MonoBehaviour {
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public LayerMask _grabMask;
    public LineRenderer _grabPointer;

    Hand _hand;

    bool _pointing;
    Throwable _pointedObject;

    private void Awake() {
        _hand = GetComponent<Hand>();
    }

    void Update() {
        _pointedObject = null;
        if (_hand.AttachedObjects.Count > 0)
            return;

        if (grabPinchAction.GetStateDown(_hand.handType)) {
            _grabPointer.gameObject.SetActive(true);
            _pointing = true;
        }

        if (_pointing) {
            RaycastHit hit;
            if (Physics.Raycast(_hand.transform.position, _hand.transform.forward, out hit, float.PositiveInfinity, _grabMask)) {
                var throwable = hit.collider.GetComponentInParent<Throwable>();
                if (throwable) {
                    _pointedObject = throwable;
                    _grabPointer.startColor = Color.red;
                    _grabPointer.endColor = Color.red;
                } else {

                    _grabPointer.startColor = Color.white;
                    _grabPointer.endColor = Color.white;
                }

                var localPosition = transform.InverseTransformPoint(hit.point);
                _grabPointer.SetPosition(1, localPosition);

            } else {

                _grabPointer.startColor = Color.white;
                _grabPointer.endColor = Color.white;

                var pos = transform.InverseTransformPoint(_hand.transform.position + _hand.transform.forward * 100f);
                _grabPointer.SetPosition(1, pos);
            }
        }

        if (grabPinchAction.GetStateUp(_hand.handType)) {
            _grabPointer.gameObject.SetActive(false);

            if (_pointedObject) {
                var grabType = _hand.GetBestGrabbingType();
                _hand.AttachObject(_pointedObject.gameObject, grabType, _pointedObject.attachmentFlags, _pointedObject.attachmentOffset);
            }

        }
    }
}