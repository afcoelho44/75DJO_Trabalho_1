using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaPredio : MonoBehaviour
{
    public GameObject somAmbiente;
    public GameObject companheiro;

    private bool IsDentro = true; // Controla se o jogador está dentro ou fora do prédio

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se é o jogador
        {
            if (!IsDentro) // Se o jogador estiver do lado de fora e entrou no prédio
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomCasa();
                IsDentro = true; // Atualiza para indicar que está dentro

                if (companheiro != null && !companheiro.activeSelf)
                {
                    companheiro.SetActive(true); // Ativa o companheiro se ele não estiver ativo

                    Companheiro compScript = companheiro.GetComponent<Companheiro>();
                    if (compScript != null)
                    {
                        compScript.Iniciar(); // Chama o método de inicialização do companheiro
                        compScript.sliderVida.gameObject.SetActive(true); // Ativa a barra de vida
                    }
                }
            }
            else // Se o jogador estiver dentro e saiu para fora
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomNatureza();
                IsDentro = false; // Atualiza para indicar que está fora
            }
        }
    }

}
