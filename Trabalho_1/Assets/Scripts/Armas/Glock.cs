using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : MonoBehaviour
{
    private Animator anim;
    private bool estahAtirando;
    private RaycastHit hit;
    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;
    public GameObject faisca;
    private AudioSource somTiro;
    private int carregador = 3;
    private int municao = 17;
    public AudioClip[] clips;
    void Start()
    {
        estahAtirando = false;
        anim = GetComponent<Animator>();
        somTiro = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("acaoOcorrendo")) {
            return;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            //enquanto a animação do tiro estiver processando
            if (!estahAtirando && municao > 0)
            {
                somTiro.clip = clips[0];
                municao--;
                estahAtirando = true;
                StartCoroutine(Atirando());
            }
            else {
                if (!estahAtirando && municao == 0 && carregador > 0)
                {
                    Recarregar();
                }
                else {
                    somTiro.clip = clips[2];
                    somTiro.time = 0;
                    somTiro.Play();
                }
            }

        }
        else {
            if (Input.GetButtonDown("Recarregar"))
            {
                if (carregador > 0 && municao < 17) {
                    Recarregar();
                }
                else
                {
                    somTiro.clip = clips[2];
                    somTiro.time = 0;
                    somTiro.Play();
                }

            }
        }
    }

    IEnumerator Atirando() { 
        //encontrar o centro da tela 
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        //definir um ponto até o centro da tela
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        anim.Play("AtirarGlock");
        somTiro.time = 0;
        somTiro.Play();
        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;
        GameObject faiscaObj = null;
        //o inimigo não precisa estar na mira exta para acertar o tiro
        if (Physics.SphereCast(ray, 0.1f, out hit)) {
            //print(hit.transform.name);
            faiscaObj = Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            if (hit.transform.tag == "Arrastar") {
                Vector3 direcaoBala = ray.direction;
                hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
            }
        }
        yield return new WaitForSeconds(0.3f);
        Destroy(efeitoTiroObj);
        Destroy(faiscaObj);
        estahAtirando = false;
    }

    private void Recarregar() {
        somTiro.clip = clips[1];
        somTiro.time = 1.05f;
        somTiro.Play();

        anim.Play("RecarregarGlock");
        municao = 17;
        carregador--;
    }
}
