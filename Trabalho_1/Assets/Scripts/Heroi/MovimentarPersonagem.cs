using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class MovimentarPersonagem : MonoBehaviour
{
    public CharacterController controle;
    public float velocidade = 6f;
    public float alturaPulo = 6f;
    public float gravidade = -20f;
    public AudioClip somPulo;
    private AudioSource audioSrc;

    public Transform checaChao;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

    Vector3 velocidadeCai;

    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado; 

    // Start is called before the first frame update
    void Start()
    {
        // No play do jogo pega um componente do tipo CharacterController que estiver associado a essa classe
        controle = GetComponent<CharacterController>();
        cameraTransform= Camera.main.transform;
        audioSrc = GetComponent<AudioSource>();
    }
    void OnDrawGizmosSelected() { 
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checaChao.position, raioEsfera);
    }

    // Update is called once per frame
    void Update()
    {
        //cria uma esfera de raioEsfera na posição do checaChao, batendo com as mascara do chao 
        // se esta em contato com chaoMask, entao retorna true
        // Aqui ele anda
        estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);

        float x = Input.GetAxis("Horizontal");

        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;

        controle.Move(mover * velocidade *  Time.deltaTime);

        ChecarBloqueioAbaixado();

        //Aqui ele pula 

        if (!levantarBloqueado && estaNoChao && Input.GetButtonDown("Jump")) {

            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            audioSrc.clip = somPulo;
            audioSrc.Play();
        }

        if (!estaNoChao) {
            velocidadeCai.y += gravidade * Time.deltaTime;
        }
        controle.Move(velocidadeCai * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            AgacharLevantar();
        }

    }

    private void AgacharLevantar() {

        if (levantarBloqueado || estaNoChao == false) {
            return;
        }
        estahAbaixado = !estahAbaixado;

        if (estahAbaixado)
        {
            controle.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraAbaixado, 0);
        }
        else {
            controle.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraEmPe, 0); 
        }
    }
    private void ChecarBloqueioAbaixado() {

        // UnityEngine.Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);
        RaycastHit hit;
        levantarBloqueado = Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f);
    }
}
