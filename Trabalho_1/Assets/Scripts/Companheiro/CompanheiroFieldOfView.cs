using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanheiroFieldOfView : MonoBehaviour
{
    public float distanciaVisao;
    [Range(0, 360)]
    public float anguloVisao;

    public bool podeVerPlayer;
    public bool podeVerInimigo;

    private GameObject player;
    private GameObject inimigoMaisProximo;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OlharPara(GameObject alvo)
    {
        Vector3 direcaoOlhar = alvo.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300); //mesma funcionalidade do Lerp
    }

    private void ProcurarAlvosVisiveis()
    {
        Collider[] alvosDentroRaio = Physics.OverlapSphere(transform.position, distanciaVisao);
        bool encontrouPlayer = false;
        bool encontrouInimigo = false;

        foreach (Collider alvo in alvosDentroRaio)
        {
            if (alvo.gameObject.CompareTag("Player"))
            {
                if (EstaNoCampoDeVisao(alvo))
                {
                    podeVerPlayer = true;
                    encontrouPlayer = true;
                    OlharPara(player);
                }
            }

            if (alvo.gameObject.CompareTag("InimigoComum"))
            {
                if (EstaNoCampoDeVisao(alvo))
                {
                    podeVerInimigo = true;
                    inimigoMaisProximo = alvo.gameObject;
                    encontrouInimigo = true;
                    OlharPara(inimigoMaisProximo); // Olha para o inimigo mais próximo
                }
            }
        }

        // Atualiza as flags caso não tenha encontrado o player ou o inimigo
        if (!encontrouPlayer)
        {
            podeVerPlayer = false;
        }
        if (!encontrouInimigo)
        {
            podeVerInimigo = false;
            inimigoMaisProximo = null;
        }
    }

    private bool EstaNoCampoDeVisao(Collider alvo)
    {
        Vector3 dirToAlvo = (alvo.transform.position - transform.position).normalized;
        dirToAlvo.y = 0;

        if (Vector3.Angle(transform.forward, dirToAlvo) < anguloVisao / 2)
        {
            float disToAlvo = Vector3.Distance(transform.position, alvo.transform.position);

            if (!Physics.Raycast(transform.position, dirToAlvo, disToAlvo))
            {
                return true;
            }
        }

        return false;
    }
    public GameObject getInimigo()
    {
        return inimigoMaisProximo;
    }
    private void FixedUpdate()
    {
        ProcurarAlvosVisiveis();
    }
    void Update()
    {

    }
}
