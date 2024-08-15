using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcoesObjeto : MonoBehaviour
{
    private IdentificarObjeto idObjetos;
    private bool pegou = false;
    
    void Start()
    {
        idObjetos = GetComponent<IdentificarObjeto>();
        
    }

    private void Pegar() {
        Destroy(idObjetos.GetObjPegar());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjPegar() != null) {
            Pegar();
        }
    }
}
