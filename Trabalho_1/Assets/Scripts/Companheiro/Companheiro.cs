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
    public float offsetSeguir = 1.5f;     // Dist�ncia para ficar atr�s ou ao lado do jogador
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
            anim.SetBool("parado", false);
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
            anim.SetBool("parado", true);
            anim.SetBool("podeAndar", false);
            
        }
    }

    private void VerificarAtaque()
    {
        GameObject inimigo = EncontrarInimigoMaisProximo();

        if (inimigo != null) {
            float distanciaInimigo = Vector3.Distance(transform.position, inimigo.transform.position);

            if (distanciaInimigo <= alcanceAtaque)
            {
                Atacar(inimigo);
            }
            else if (distanciaInimigo >= alcanceAtaque + 1) {
                anim.SetBool("pararAtaque", true);
            }
        }
    }

    private GameObject EncontrarInimigoMaisProximo() {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("LevarDano ");
        GameObject inimigoMaisProx = null;

        float menorDistancia = Mathf.Infinity;

        foreach (GameObject inimigo in inimigos) {
            float distanciaPlayer = Vector3.Distance(player.transform.position, inimigo.transform.position);

            if (distanciaPlayer < menorDistancia) { 
                menorDistancia = distanciaPlayer;
                inimigoMaisProx = inimigo;
            }
        }
        return inimigoMaisProx;
    }
    private void Atacar(GameObject inimigo)
    {
        agente.isStopped = true;
        anim.SetTrigger("ataque");
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", false);
        anim.SetBool("parado", false);
        CorrigirRigiEntrar();
        Debug.Log("Atacando inimigo: " + inimigo.name); // Log para confirmar o ataque

        // L�gica de dano ao alvo, assumindo que o alvo tem um m�todo para levar dano
        ILevarDano componenteDano = inimigo.GetComponent<ILevarDano>();
        if (componenteDano != null)
        {
            componenteDano.LevarDano(danoAtaque);
        }
        else
        {
            Debug.LogWarning("O inimigo n�o implementa ILevarDano!");
        }
        anim.ResetTrigger("ataque");
    }
    private void CorrigirRigiEntrar() { 
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
