using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class IdentificarObjeto : MonoBehaviour
{
    private float distanciaAlvo;
    private GameObject objArrastar, objPegar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // será executado a cada 5 frames
        // como pode chegar a 60 frames/segundo quebraria o jogo

        if (Time.frameCount % 5 == 0) {
            objArrastar = null;
            objPegar = null;

            int ignorarLayer = 7; // ignoreplayercast
            ignorarLayer = 1 << ignorarLayer;
            ignorarLayer = ~ignorarLayer;

            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5, ignorarLayer)) { 
                distanciaAlvo = hit.distance;

                if (hit.transform.gameObject.tag == "Arrastar") {
                    objArrastar = hit.transform.gameObject;
                   // print("Arrastar "+ objArrastar);
                }
                if (hit.transform.gameObject.tag == "Pegar")
                {
                    objPegar = hit.transform.gameObject;
                   // print("Pegar " + objPegar);
                }
            }

        }
    }

    public float GetDistanciaAlvo() { 
        return distanciaAlvo;
    }

    public GameObject GetObjArrastar() { 
        return objArrastar;
    }
    public GameObject GetObjPegar() {
        return objPegar;
    }
}
