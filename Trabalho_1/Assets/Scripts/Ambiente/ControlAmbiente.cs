using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAmbiente : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip somNatureza;
    public AudioClip somPorta;
    public AudioClip somCasa;
    public AudioClip somIgreja;

    private AudioSource audioAmbiente; // Som ambiente cont�nuo
    private AudioSource audioEfeitos;  // Sons curtos como porta

    void Start()
    {
        // Configura dois AudioSources: um para o ambiente e outro para efeitos sonoros
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioAmbiente = audioSources[0];
        audioEfeitos = audioSources[1];

        // Configura��es do som de ambiente
        audioAmbiente.loop = true;
        audioAmbiente.clip = somCasa; // Defina o som inicial
        audioAmbiente.Play();
    }

    public void SomPorta()
    {
        // Sons de porta ou outros efeitos n�o interrompem o ambiente
        audioEfeitos.loop = false;
        audioEfeitos.clip = somPorta;
        audioEfeitos.Play();
    }

    public void SomNatureza()
    {
        audioAmbiente.loop = true;
        audioAmbiente.clip = somNatureza;
        audioAmbiente.Play();
    }

    public void SomCasa()
    {
        audioAmbiente.loop = true;
        audioAmbiente.clip = somCasa;
        audioAmbiente.Play();
    }

    public void SomIgreja()
    {
        audioAmbiente.loop = true;
        audioAmbiente.clip = somIgreja;
        audioAmbiente.Play();
    }
    public void PararSomAmbiente()
    {
        audioAmbiente.Stop();
    }
    void Update()
    {
        // Aqui voc� pode adicionar l�gicas para mudar o som de ambiente com base na localiza��o, se necess�rio
    }
}
