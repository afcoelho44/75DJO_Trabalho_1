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
    public AudioSource audio;
    public AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
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

    private void OlharParaJogador() { 
        Vector3 direcaoOlhar = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300); //mesma funcionalidade do Lerp
    }


    // Update is called once per frame
    void Update()
    {
        VaiAtrasJoagdor();
        OlharParaJogador();

        if (vida <= 0) {
            Morreu();
        }
    }
    public void LevarDano(int dano) {
        vida -= dano;
        agente.isStopped = false;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
    }

    private void Morreu() {
        audio.clip = clips[0];
        audio.Play();

        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", true);

        anim.SetBool("morreu", true);

        this.enabled = false; //para de executar esse script

    }
    public void DarDano() {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-10);
    }
}
