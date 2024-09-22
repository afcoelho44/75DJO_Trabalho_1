using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaPredio : MonoBehaviour
{
    public GameObject somAmbiente;
    public GameObject companheiro;

    private bool IsDentro = true; // Controla se o jogador est� dentro ou fora do pr�dio

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se � o jogador
        {
            if (!IsDentro) // Se o jogador estiver do lado de fora e entrou no pr�dio
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomCasa();
                IsDentro = true; // Atualiza para indicar que est� dentro

                if (companheiro != null && !companheiro.activeSelf)
                {
                    companheiro.SetActive(true); // Ativa o companheiro se ele n�o estiver ativo

                    Companheiro compScript = companheiro.GetComponent<Companheiro>();
                    if (compScript != null)
                    {
                        compScript.Iniciar(); // Chama o m�todo de inicializa��o do companheiro
                        compScript.sliderVida.gameObject.SetActive(true); // Ativa a barra de vida
                    }
                }
            }
            else // Se o jogador estiver dentro e saiu para fora
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomNatureza();
                IsDentro = false; // Atualiza para indicar que est� fora
            }
        }
    }

}
