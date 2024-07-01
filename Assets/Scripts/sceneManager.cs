using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    private int nextSceneIndex;
    // El codigo informa cual es la siguiente escena
    private void Start()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    }
    // Si el jugador coliciona con este pasa a la siguiente escena
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    public void ChangeScene(int sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
