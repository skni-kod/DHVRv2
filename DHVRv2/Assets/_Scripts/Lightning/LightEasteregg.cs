using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEasteregg : MonoBehaviour
{
    public float _setIntensity = 10.0f;

    private bool _isDimming;
    private short _timesShot = 0;
    private bool _dimming = false;
    public float _dimmingInterval = 0.1f;
    private float _dimmingTime = 0.0f;

    private void Awake()
    {
        RocketLauncher.OnRocketLaunched += this.OnRocketLaunched;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnDestroy()
    {
        RocketLauncher.OnRocketLaunched -= this.OnRocketLaunched;
    }

    // Update is called once per frame
    void Update()
    {
        if(_dimming)
        {
            _dimmingTime += Time.deltaTime;
            if(_dimmingTime >= _dimmingInterval)
            {
                _dimmingTime = 0.0f;
                this.GetComponent<Light>().intensity -= 0.01f;
                if(this.GetComponent<Light>().intensity <= 1.0f)
                {
                    this.GetComponent<Light>().intensity = 1.0f;
                    _dimming = false;
                }
            }
        }
    }

    public void OnRocketLaunched(RocketLauncher rocketLauncher)
    {
        Debug.Log(_timesShot);
        float dot = Vector3.Dot(rocketLauncher.transform.forward, transform.forward);
        if (dot <= -0.85f)
        {
            _timesShot++;
        }

        if(_timesShot >= 5)
        { 
            if (this.GetComponent<Light>() != null)
            {
                this.GetComponent<Light>().intensity = _setIntensity;
                _dimming = true;
            }

            _timesShot = 0;
        }
    }
}
