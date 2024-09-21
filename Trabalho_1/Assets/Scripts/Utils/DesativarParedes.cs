using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesativarParedes : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] paredes;
    public GameObject local;
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        GerenciadorInimigos g = local.GetComponent<GerenciadorInimigos>();
        if (g.inimigosMortos == g.inimigos.Length) {
            for (int i = 0; i < paredes.Length; i++)
            {
                paredes[i].SetActive(false);
            }
        }
    }
}
