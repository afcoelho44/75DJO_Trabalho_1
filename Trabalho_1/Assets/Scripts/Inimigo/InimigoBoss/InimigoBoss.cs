using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigoBoss : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaDoAtaque = 2.0f;
    public int vida = 100;
    public GameObject inimigo;
    public AudioClip somMorte;
    public AudioClip somPasso;
    public AudioClip somGrunir;
    private AudioSource audioSrc;
    private FieldOfView fov;
    private PatrulharAleatorio pal;
    public GameObject local;


    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        fov = GetComponent<FieldOfView>();
        pal= GetComponent<PatrulharAleatorio>();
    }

    private void VaiAtrasJogador() {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaDoPlayer < distanciaDoAtaque)
        {
            agente.isStopped = true;
           // Debug.Log("Ataque");

            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            CorrigirRigiEntrar();
        }
        //player se afastou
        if (distanciaDoPlayer >= distanciaDoAtaque + 1) {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
        }
        if (anim.GetBool("podeAndar")) { 
            agente.isStopped= false;
            agente.SetDestination(player.transform.position);
        }
    }
    private void OlharParaJogador() { 
        Vector3 direcaoOlhar = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }
    private void CorrigirRigiEntrar() { 
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
    public void LevarDano(int dano) {
        vida -= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
    }
    private void Morrer() {
        local.GetComponent<GerenciadorInimigos>().InimigoBossMorto();
        audioSrc.clip = somMorte;
        audioSrc.Play();
        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", true);

        anim.SetBool("morreu", true);

        StartCoroutine(EsperarFimDaAnimacao());
        player.GetComponent<MovimentarPersonagem>().AtualizarScore(30);
        this.enabled = false;
      
    }
    private void DestruirInimigo()
    {
        Destroy(inimigo);
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
        DestruirInimigo();
    }

    public void DarDano() {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-20);
    }
    void Update()
    {
        //VaiAtrasJogador();
        //OlharParaJogador();

        if (vida <= 0) {
            Morrer();
            return;
        }
        if (fov.podeVerPlayer)
        {
            VaiAtrasJogador();
        }
        else {
            anim.SetBool("pararAtaque",true);
            CorrigirRigiSair();
            agente.isStopped = false;
            pal.Andar();

        }
    }
    public void Passo() {
        audioSrc.PlayOneShot(somPasso, 0.05f);
    }
    public void Grunhir()
    {
        audioSrc.PlayOneShot(somGrunir);
    }
}
