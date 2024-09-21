using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboFimDeJogo : MonoBehaviour, IPegavel
{
    private GameObject player;
    public void Pegar()
    {
        player.GetComponent<MovimentarPersonagem>().FimDeJogo();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
