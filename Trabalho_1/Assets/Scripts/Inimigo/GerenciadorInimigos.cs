using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorInimigos : MonoBehaviour
{
    public GameObject[] inimigos; // Array de inimigos (até 10 inimigos)
    private int inimigosMortos = 0; // Contador de inimigos mortos
    public int inimigosAtivos = 3; // Inimigos ativos inicialmente

    void Start()
    {
        // Ativa os 3 primeiros inimigos no início
        for (int i = 0; i < inimigosAtivos; i++)
        {
            inimigos[i].SetActive(true);
        }

        // Desativa o resto dos inimigos no começo
        for (int i = inimigosAtivos; i < inimigos.Length; i++)
        {
            inimigos[i].SetActive(false);
        }
    }

    // Método para ser chamado quando um inimigo morrer
    public void InimigoMorto()
    {
        inimigosMortos++;

        // Verifica se a cada 3 inimigos mortos, um novo deve ser ativado
        if (inimigosAtivos < inimigos.Length)
        {
            // Ativa um novo inimigo do array
            inimigos[inimigosAtivos].SetActive(true);
            inimigosAtivos++;
        }
    }
}
