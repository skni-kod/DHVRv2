using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PumpkinManager : MonoBehaviour
{

    List<Pumpkin> pumpkins;

    private void Awake()
    {
        Pumpkin.OnPumpkinDestroyed += this.OnPumpkinDestroyed;
        BigDuckController.OnBigDuckDeath += this.OnBigDuckDeath;
    }

    private void OnDestroy()
    {
        Pumpkin.OnPumpkinDestroyed -= this.OnPumpkinDestroyed;
        BigDuckController.OnBigDuckDeath -= this.OnBigDuckDeath;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPumpkinDestroyed(Pumpkin pumpkin)
    {
        //pumpkins.Add(pumpkin);
    }

    void OnBigDuckDeath(BigDuckController bigDuck)
    {
        //foreach(Pumpkin pumpkin in pumpkins)
        //{
        //    pumpkin.gameObject.SetActive(true);
        //}
    }
}
