using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAmbiente : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip somNatureza;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = somNatureza;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
