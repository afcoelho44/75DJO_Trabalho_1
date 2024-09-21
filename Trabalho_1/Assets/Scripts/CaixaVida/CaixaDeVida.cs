using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaDeVida : MonoBehaviour, IPegavel
{

    public void Pegar()
    {

        MovimentarPersonagem player = GameObject.FindWithTag("Player").GetComponent<MovimentarPersonagem>();
        Companheiro mascote = GameObject.FindWithTag("Mascote").GetComponent<Companheiro>();
        player.AtualizarVida(30);
        mascote.AtualizarVida(10);
       
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
