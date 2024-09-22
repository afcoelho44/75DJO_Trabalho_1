using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaPredio : MonoBehaviour
{
    public GameObject somAmbiente;
    public GameObject companheiro;

    private void OnTriggerEnter(Collider other)
    {
        somAmbiente.GetComponent<ControlAmbiente>().SomNatureza();
    }
    private void OnCollisionExit(Collision collision)
    {
        somAmbiente.GetComponent<ControlAmbiente>().SomCasa();
    }

}
