using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void IniciarJogo()
    {
        SceneManager.LoadScene(1);   
    }

    // Update is called once per frame
    public void SairJogo()
    {
        Application.Quit();
    }
}
