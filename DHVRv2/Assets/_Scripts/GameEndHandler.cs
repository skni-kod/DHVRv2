using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightDayData {
    public Vector3 rotation;
    public float intensity;
}

public class GameEndHandler : MonoBehaviour {
    [Header("Visual Objects")]
    public ParticleSystem[] _fireworks;
    public GameObject[] _friedDucks;

    [Header("Lightning")]
    public Light _sunLight;

    public LightDayData _dayLightData;
    public LightDayData _eveningLightData;
    public AnimationCurve _lightAnimationCurve;

    public void GameWon() {
        for (int i = 0; i < _fireworks.Length; i++) {
            _fireworks[i].Play();
        }

        for (int i = 0; i < _friedDucks.Length; i++) {
            _friedDucks[i].SetActive(true);
        }

        StartCoroutine(SunAnimation(true));
    }

    public void GameLost() {

        //Do something I guess...?
    }

    public void GameReset() {
        for (int i = 0; i < _fireworks.Length; i++) {
            _fireworks[i].Stop();
        }

        for (int i = 0; i < _friedDucks.Length; i++) {
            _friedDucks[i].SetActive(false);
        }

        StartCoroutine(SunAnimation(false));
    }

    IEnumerator SunAnimation(bool evening) {
        float percent = 0;

        float animationTime = 2f;
        Quaternion startRotation;
        Quaternion targetRotation;

        float startIntensity;
        float targetIntensity;

        if (evening) {
            startRotation = Quaternion.Euler(_dayLightData.rotation);
            targetRotation = Quaternion.Euler(_eveningLightData.rotation);

            startIntensity = _dayLightData.intensity;
            targetIntensity = _eveningLightData.intensity;
        } else {

            targetRotation = Quaternion.Euler(_dayLightData.rotation);
            startRotation = Quaternion.Euler(_eveningLightData.rotation);

            targetIntensity = _dayLightData.intensity;
            startIntensity = _eveningLightData.intensity;
        }

        while (percent <= 1) {
            var p = _lightAnimationCurve.Evaluate(percent);
            _sunLight.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, p);
            _sunLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, p);

            percent += 1 / animationTime * Time.deltaTime;

            Debug.Log(percent);
            yield return null;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Set Light As Day Lightning")]
    public void SetLightAsDayLightning() {
        _dayLightData.rotation = _sunLight.transform.rotation.eulerAngles;
        _dayLightData.intensity = _sunLight.intensity;
    }

    [ContextMenu("Set Light As Evening Lightning")]
    public void SetLightAsEveningLightning() {
        _eveningLightData.rotation = _sunLight.transform.rotation.eulerAngles;
        _eveningLightData.intensity = _sunLight.intensity;
    }

    [ContextMenu("Reset Light To Day")]
    public void ResetLightToDay() {
        _sunLight.transform.rotation = Quaternion.Euler(_dayLightData.rotation);
        _sunLight.intensity = _dayLightData.intensity;
    }

    [ContextMenu("Reset Light To Evening")]
    public void ResetLightToEvening() {
        _sunLight.transform.rotation = Quaternion.Euler(_eveningLightData.rotation);
        _sunLight.intensity = _eveningLightData.intensity;
    }
#endif
}