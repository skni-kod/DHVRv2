using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug1 : MonoBehaviour
{
    public GameObject _duckObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float h = _duckObject.GetComponent<BigDuckController>()._currentHealth;
        this.transform.localScale = new Vector3((float)((_duckObject.GetComponent<BigDuckController>()._currentHealth / _duckObject.GetComponent<BigDuckController>()._maxHealth) * 0.3), 1f, 0.11f);
    }
}
