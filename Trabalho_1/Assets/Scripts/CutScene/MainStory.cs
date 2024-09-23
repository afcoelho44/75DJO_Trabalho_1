using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
    public void LoadNextScene()
    {
        Debug.Log("Recebeu o sinal");
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
