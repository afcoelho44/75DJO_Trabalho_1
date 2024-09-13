using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companheiro : MonoBehaviour
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaSeguir = 10.0f; // Dist�ncia a partir da qual o animal come�a a seguir o jogador
    public float distanciaParar = 2.0f;   // Dist�ncia m�nima ao jogador para parar de se mover
    public float offsetSeguir = 100.0f;     // Dist�ncia para ficar atr�s ou ao lado do jogador
    public float alcanceAtaque = 1.5f;    // Alcance para o companheiro atacar
    public int danoAtaque = 10;           // Dano causado pelo ataque

    //public AudioSource audioSrc;
    //public AudioClip somPasso;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        //audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        SeguirJogador();
        VerificarAtaque();
    }

    private void SeguirJogador()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaDoPlayer < distanciaSeguir && distanciaDoPlayer > distanciaParar)
        {
            agente.isStopped = false;
            // Calcula uma posi��o offset atr�s do jogador usando a dire��o oposta � frente do jogador
            Vector3 direcaoParaTras = -player.transform.forward * offsetSeguir;
            Vector3 posicaoDestino = player.transform.position + direcaoParaTras;
            anim.SetBool("podeAndar", true);

            agente.SetDestination(posicaoDestino);
            

            // Toca som de passos se necess�rio
            //if (!audioSrc.isPlaying)
            //{
            //    audioSrc.PlayOneShot(somPasso, 0.5f);
            //}
        }
        else
        {
            agente.isStopped = true;
            anim.SetBool("podeAndar", false);
            
        }
    }

    private void VerificarAtaque()
    {
        // Supondo que voc� define o alvo manualmente ou com alguma l�gica de detec��o
        GameObject inimigo = GameObject.FindWithTag("LevarDano ");
        if (inimigo != null)
        {
            float distanciaDoAlvo = Vector3.Distance(transform.position, inimigo.transform.position);

            if (distanciaDoAlvo <= alcanceAtaque)
            {
                Atacar(inimigo);
            }
            //Inimigo se afastou
            if (distanciaDoAlvo >= alcanceAtaque + 1) {
                anim.SetBool("pararAtaque", true);
            }
        }
    }

    private void Atacar(GameObject inimigo)
    {
        agente.isStopped = true;
        anim.SetTrigger("ataque");
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", false);
        
        // L�gica de dano ao alvo, assumindo que o alvo tem um m�todo para levar dano
        ILevarDano componenteDano = inimigo.GetComponent<ILevarDano>();
        if (componenteDano != null)
        {
            componenteDano.LevarDano(danoAtaque);
        }
    }
    private void CorrigirRigiEntrar() { 
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
