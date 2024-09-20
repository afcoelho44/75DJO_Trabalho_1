using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigoComum : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaDoAtaque = 2.0f;
    public int vida = 50;
    public AudioSource audioSrc;
    public AudioClip somMorte;
    public AudioClip somPasso;
    public FieldOfView fov;
    private PatrulharAleatorio pal;
    public GameObject inimigo;
    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        fov = GetComponent<FieldOfView>();
        pal = GetComponent<PatrulharAleatorio>();

    }
    private void CorrigirRigiEntrar() { 
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void VaiAtrasJoagdor() {
        float distanciaDoPlayer = Vector3.Distance(transform.position,player.transform.position );
        if (distanciaDoPlayer < distanciaDoAtaque)
        {
            agente.isStopped = true;
           // Debug.Log("Ataque");

            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            CorrigirRigiEntrar();
        }
        if(distanciaDoPlayer >= distanciaDoAtaque + 1) {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
        }
        if (anim.GetBool("podeAndar")) {
            agente.isStopped= false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    //private void OlharParaJogador() { 
    //    Vector3 direcaoOlhar = player.transform.position - transform.position;
    //    Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300); //mesma funcionalidade do Lerp
    //}


    // Update is called once per frame
    void Update()
    {
       // VaiAtrasJoagdor();
        //OlharParaJogador();

        if (vida <= 0) {
            Morreu();
            //float duracao = anim.GetCurrentAnimatorStateInfo(0).length;
            //Destroy(inimigo, duracao);
            return;
        }
        if (fov.podeVerPlayer)
        {
            VaiAtrasJoagdor();
        }
        else {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
            agente.isStopped = false;
            pal.Andar();
        }
    }
    public void LevarDano(int dano) {
        vida -= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
    }

    private void Morreu() {
        audioSrc.clip = somMorte;
        audioSrc.Play();

        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", true);

        anim.SetBool("morreu", true);

        StartCoroutine(EsperarFimDaAnimacao());
        player.GetComponent<MovimentarPersonagem>().AtualizarScore(10);
        this.enabled = false; //para de executar esse script
        fov.enabled = false;
        
    }
    public void DarDano() {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-10);
    }
    public void Passo() {
        //ideal para sons repetitivos
        audioSrc.PlayOneShot(somPasso, 0.5f);
    }
    private void DestruirInimigo() {
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

}
