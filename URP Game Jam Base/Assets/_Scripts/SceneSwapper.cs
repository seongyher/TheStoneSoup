using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}