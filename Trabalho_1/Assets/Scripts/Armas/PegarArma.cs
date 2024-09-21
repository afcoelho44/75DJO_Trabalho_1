using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarArma : MonoBehaviour, IPegavel
{
    public GameObject arma;
    public void Pegar()
    {
        arma.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
