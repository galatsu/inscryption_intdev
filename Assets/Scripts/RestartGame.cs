using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public string sceneToLoad;

    public void SceneChange()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
