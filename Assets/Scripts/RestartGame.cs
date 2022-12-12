using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject backgroundmusic;
    string sceneName = "";

    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    public void SceneChange()
    {
        if (sceneName == "Startup") { DontDestroyOnLoad(backgroundmusic); }
        else
        {
            AudioSource startmusic = GameObject.FindGameObjectWithTag("StartMusic").GetComponent<AudioSource>();
            startmusic.clip = null;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
