using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaTemplo : MonoBehaviour
{
    public GameObject somAmbiente;
    private bool IsDentro = false; // Controla se o jogador está dentro ou fora da casa

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se é o jogador
        {
            if (!IsDentro) // Se o jogador estiver do lado de fora e entrou na casa
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomIgreja();
                IsDentro = true; // Atualiza para indicar que está dentro
            }
            else // Se o jogador estiver dentro e saiu para fora
            {
                somAmbiente.GetComponent<ControlAmbiente>().SomNatureza();
                IsDentro = false; // Atualiza para indicar que está fora
            }
        }
    }
}
