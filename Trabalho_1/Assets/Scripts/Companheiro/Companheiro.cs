using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companheiro : MonoBehaviour, ILevarDano
{
    public float intervaloAtaque = 1.5f; // Intervalo de tempo entre os ataques (em segundos)
    private float tempoUltimoAtaque = 0f; // Armazena o tempo do último ataque
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaSeguir = 3.0f; // Distância a partir da qual o animal começa a seguir o jogador
    public float distanciaParar = 2.0f;   // Distância mínima ao jogador para parar de se mover
    public float offsetSeguir = 1.5f;     // Distância para ficar atrás ou ao lado do jogador
    public float alcanceAtaque = 2.0f;    // Alcance para o companheiro atacar
    public int danoAtaque = 3;           // Dano causado pelo ataque
    public CompanheiroFieldOfView fov;
    private Vector3 ultimaPosicaoPlayer;
    public int vida = 30;
    public GameObject companheiro;

    //public AudioSource audioSrc;
    //public AudioClip somPasso;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        fov = GetComponent<CompanheiroFieldOfView>();
        ultimaPosicaoPlayer = player.transform.position;

        //audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (vida<=0) {
            Morreu();
            return;
        }

        if (fov.podeVerInimigo)
        {
            SeguirInimigo();
        }
        else
        if ( !fov.podeVerInimigo)
        {
            anim.ResetTrigger("ataque");
            VerificarMovimentoJogador();

        }
   
    }

    private void VerificarMovimentoJogador()
    {
        Vector3 direcaoMovimento = player.transform.position - ultimaPosicaoPlayer;

        if (direcaoMovimento.magnitude > 0.01f) // Se o jogador estiver se movendo
        {
            SeguirJogador();
        }
        else
        {
            PararCompanheiro();
        }

        ultimaPosicaoPlayer = player.transform.position; // Atualiza a posição do player
    }

    private void SeguirJogador()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaDoPlayer < distanciaSeguir && distanciaDoPlayer > distanciaParar)
        {
            agente.isStopped = false;

            // Calcula uma posição offset atrás do jogador usando a direção oposta à frente do jogador
            Vector3 direcaoParaTras = -player.transform.forward * offsetSeguir;
            Vector3 posicaoDestino = player.transform.position + direcaoParaTras;

            anim.SetBool("parado", false);
            anim.SetBool("podeAndar", true);
            CorrigirRigiEntrar();
            agente.SetDestination(posicaoDestino);
        }
    }

    private void PararCompanheiro()
    {
        agente.isStopped = true;
        anim.SetBool("parado", true);
        anim.SetBool("podeAndar", false);
        CorrigirRigiSair();
    }

    private void SeguirInimigo() {
        GameObject inimigo = fov.getInimigo();
        if (inimigo != null )
        {
            if (inimigo.GetComponent<InimigoComum>().vida > 0)
            {
                float distanciaDoInimigo = Vector3.Distance(transform.position, inimigo.transform.position);

                if (distanciaDoInimigo < alcanceAtaque && Time.time >= tempoUltimoAtaque + intervaloAtaque)
                {
                    agente.isStopped = false;
                    anim.SetTrigger("ataque");
                    anim.SetBool("podeAndar", false);
                    anim.SetBool("pararAtaque", false);
                    CorrigirRigiEntrar();
                    inimigo.GetComponent<InimigoComum>().LevarDano(3);

                    // Atualiza o tempo do último ataque
                    tempoUltimoAtaque = Time.time;

                    // Se a vida do inimigo chegar a 0 após o ataque, interrompe o ataque
                    if (inimigo.GetComponent<InimigoComum>().vida <= 0)
                    {
                        anim.SetBool("pararAtaque", true);
                        CorrigirRigiSair();
                    }
                }
                if (distanciaDoInimigo >= alcanceAtaque + 1)
                {
                    anim.SetBool("pararAtaque", true);
                    CorrigirRigiSair();
                }
                if (anim.GetBool("podeAndar"))
                {
                    agente.isStopped = false;
                    agente.SetDestination(inimigo.transform.position);
                    anim.ResetTrigger("ataque");
                }
            }
            else
            {
                // Se o inimigo estiver morto, para o ataque e reseta as animações
                anim.SetBool("pararAtaque", true);
                CorrigirRigiSair();
                anim.ResetTrigger("ataque");
                agente.isStopped = false; // O companheiro pode seguir outros alvos ou se mover
            }
        }
        else {
            agente.isStopped = false;
            anim.ResetTrigger("ataque");
        }
    }
    private void CorrigirRigiEntrar() { 
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void LevarDano(int dano)
    {
        vida-=dano;
        agente.isStopped=true;
        anim.SetTrigger("levouDano");
        anim.SetBool("podeAndar", false);
    }
    private void Morreu() {
        //audioSrc.clip = somMorte;
        //audioSrc.Play();

        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", true);

        anim.SetBool("morreu", true);

        StartCoroutine(EsperarFimDaAnimacao());
      
        this.enabled = false; //para de executar esse script
        fov.enabled = false;
    }
    private void DestruirCompanheiro()
    {
        Destroy(companheiro);
    }
    private IEnumerator EsperarFimDaAnimacao()
    {
        // Obtém o estado atual da animação
        AnimatorStateInfo estadoAnimacao = anim.GetCurrentAnimatorStateInfo(0);

        // Espera até que o estado atual seja o da animação de morte e ela tenha terminado
        while (!estadoAnimacao.IsName("Morrendo") || estadoAnimacao.normalizedTime < 1.0f)
        {
            estadoAnimacao = anim.GetCurrentAnimatorStateInfo(0);
            yield return null; // Espera o próximo frame
        }

        // Depois que a animação termina, destrói o inimigo
        DestruirCompanheiro();
    }
}
