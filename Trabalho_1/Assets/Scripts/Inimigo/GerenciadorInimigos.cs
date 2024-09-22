using SunTemple;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorInimigos : MonoBehaviour
{
    public GameObject[] inimigos; // Array de inimigos (até 10 inimigos)
    public int inimigosMortos = 0; // Contador de inimigos mortos
    public int inimigosAtivos = 3; // Inimigos ativos inicialmente
    public GameObject boss;
    public GameObject door;
    public bool IsNotActiveBoss;
    //public GameObject companheiro;
    public GameObject somAmbiente;
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

        if (IsNotActiveBoss) {
            boss.SetActive(false);
        }
        //if (companheiro != null) {
            //companheiro.SetActive(false);
            //companheiro.GetComponent<Companheiro>().sliderVida.gameObject.SetActive(false);
        //}
        if (door != null) {
            door.GetComponent<Door>().IsLocked = true;
            door.GetComponent<Outline>().OutlineWidth = 0f;
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
        else  
        if(inimigosMortos == inimigos.Length)
        {
            AtivarBoss();
        }
    }
    public void AtivarBoss() {
        if (boss != null) { 
            boss.SetActive(true);
        }
    }
    public void InimigoBossMorto() {
        liberarPorta();
        //AtivarCompanheiro();
    }
    public void liberarPorta() {
        if(door!= null)
        door.GetComponent<Door>().IsLocked = false;
        door.GetComponent<Outline>().OutlineWidth = 5f;
        somAmbiente.GetComponent<ControlAmbiente>().SomPorta();

    }
    //public void AtivarCompanheiro() {
    //    if(companheiro != null) {
    //        companheiro.SetActive(true);
    //        companheiro.GetComponent<Companheiro>().sliderVida.gameObject.SetActive(true);
    //    }
        
    //}
}
