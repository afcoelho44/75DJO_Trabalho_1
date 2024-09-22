using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaCasa : MonoBehaviour
{
    public GameObject somAmbiente;
    private bool dentroDaCasa = false; // Controla se o jogador está dentro ou fora da casa

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se é o jogador
        {
            if (!dentroDaCasa) // Se o jogador estiver do lado de fora e entrou na casa
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomCasa();
                dentroDaCasa = true; // Atualiza para indicar que está dentro
            }
            else // Se o jogador estiver dentro e saiu para fora
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomNatureza();
                dentroDaCasa = false; // Atualiza para indicar que está fora
            }
        }
    }
}
