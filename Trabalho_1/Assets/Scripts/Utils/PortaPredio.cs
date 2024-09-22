using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaPredio : MonoBehaviour
{
    public GameObject somAmbiente;
    public GameObject companheiro;

    private bool IsDentro;

    // Inicializa o estado ao iniciar o jogo ou reiniciar a cena
    void Start()
    {
        IsDentro = true; // O jogador começa dentro da casa
        somAmbiente.GetComponent<ControlAmbiente>().SomCasa(); 
        companheiro.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se é o jogador
        {
            if (!IsDentro) // Se o jogador estava fora e entrou na casa
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomCasa();
                IsDentro = true; // Atualiza para indicar que está dentro da casa
            }
            else // Se o jogador estava dentro e saiu para fora
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomNatureza();
                IsDentro = false; // Atualiza para indicar que está fora da casa
                AtivarCompanheiro(); // Ativa o companheiro ao sair da casa
            }
        }
    }

    // Método para ativar o companheiro quando o jogador sai da casa
    private void AtivarCompanheiro()
    {
        if (!companheiro.activeSelf)
        {
            companheiro.SetActive(true); // Ativa o companheiro
            Companheiro compScript = companheiro.GetComponent<Companheiro>();
            if (compScript != null)
            {
                compScript.Iniciar(); // Inicia o comportamento do companheiro
                compScript.sliderVida.gameObject.SetActive(true); // Ativa a barra de vida do companheiro
            }
        }
    }
}
