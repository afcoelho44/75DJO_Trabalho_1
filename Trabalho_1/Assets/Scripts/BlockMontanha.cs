using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMontanha : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject texto;

    void OnTriggerEnter(Collider other)
    {
        texto.SetActive(false);

    }

    void OnTriggerExit(Collider other)
    {
        texto.SetActive(true);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
