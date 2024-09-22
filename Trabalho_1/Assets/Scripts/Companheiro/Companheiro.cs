using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public Slider sliderVida;
    public AudioSource audioSrc;
    public AudioClip somMorte;
    public AudioClip somPasso;
    public AudioClip somAtaque;

   

    void Start()
    {
        Iniciar();
    }
    public void Iniciar()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        fov = GetComponent<CompanheiroFieldOfView>();
        ultimaPosicaoPlayer = player.transform.position;

        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = false;
    }

    void Update()
    {
        if (vida <= 0)
        {
            Morreu();
            return;
        }
        else {
            float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanciaDoPlayer > distanciaSeguir)
            {
                EsperarJogador(); // Fica parado até o jogador se aproximar
            }
            else
                    if (fov.podeVerCaixa && player.GetComponent<MovimentarPersonagem>().vida < 100)
            {
                PegarVida();
            }
            else
            if (fov.podeVerInimigo)
                {
                    SeguirInimigo();
                }
            else
            if (fov.podeVerPlayer && !fov.podeVerInimigo && !fov.podeVerCaixa)
            {
                anim.ResetTrigger("ataque");
                VerificarMovimentoJogador();

            }

        }
   
    }

    private void EsperarJogador()
    {
        // Faz o companheiro ficar parado, esperando o jogador
        PararCompanheiro();
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

            // Tocar som de passos em loop apenas se não estiver tocando
            if (!audioSrc.isPlaying)
            {
                audioSrc.loop = true; // Habilitar o loop para som de passos
                audioSrc.clip = somPasso;
                audioSrc.Play();
            }
            agente.SetDestination(posicaoDestino);
        }
    }

    private void PararCompanheiro()
    {
        agente.isStopped = true;
        anim.SetBool("parado", true);
        anim.SetBool("podeAndar", false);

        if (audioSrc.isPlaying && audioSrc.clip == somPasso)
        {
            audioSrc.Stop(); // Parar o som de passos
        }

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
                    audioSrc.loop = false;
                    audioSrc.clip = somAtaque;
                    audioSrc.Play();
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
                    // Tocar som de passos em loop apenas se não estiver tocando
                    if (!audioSrc.isPlaying)
                    {
                        audioSrc.loop = true; // Habilitar o loop para som de passos
                        audioSrc.clip = somPasso;
                        audioSrc.Play();
                    }
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
        audioSrc.loop = false;
        audioSrc.clip = somMorte;
        audioSrc.Play();
        AtualizarVida(-dano);
        agente.isStopped=true;
        anim.SetTrigger("levouDano");
        anim.SetBool("podeAndar", false);
    }
    private void Morreu() {
        audioSrc.loop=false;
        audioSrc.clip = somMorte;
        audioSrc.Play();

        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("parado", false);
        anim.SetBool("pararAtaque", true);

        anim.SetBool("morreu", true);
        sliderVida.gameObject.SetActive(false);
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
    public void AtualizarVida(int novaVida) {
        vida = Mathf.CeilToInt(Mathf.Clamp(vida + novaVida, 0, 100));

        sliderVida.value= vida; 
    }
    private void PegarVida() {
        GameObject caixa = fov.getCaixaDeVida();  // Obtenha a caixa de vida visível no campo de visão

        if (caixa != null)
        {
            float distanciaDaCaixa = Vector3.Distance(transform.position, caixa.transform.position);

            // Se o companheiro estiver perto da caixa de vida, pega a caixa
            if (distanciaDaCaixa < 1.0f)
            {
                caixa.GetComponent<CaixaDeVida>().Pegar();
                Destroy(caixa);
            }
            else
            {
                // Caso contrário, continua se movendo até a caixa de vida
                agente.isStopped = false;
                agente.SetDestination(caixa.transform.position);
                anim.SetBool("podeAndar", true);
                // Tocar som de passos em loop apenas se não estiver tocando
                if (!audioSrc.isPlaying)
                {
                    audioSrc.loop = true; // Habilitar o loop para som de passos
                    audioSrc.clip = somPasso;
                    audioSrc.Play();
                }
                CorrigirRigiEntrar();
            }
        }
        else
        {
            anim.SetBool("parado", true);
            anim.SetBool("podeAndar", false);
            if (audioSrc.isPlaying && audioSrc.clip == somPasso)
            {
                audioSrc.Stop(); // Parar o som de passos
            }
            CorrigirRigiSair();
            agente.isStopped = true; // Para o agente se a caixa não for encontrada
        }
    }
}
