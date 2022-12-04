using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Balance : MonoBehaviour
{
    public int balanceofplayers = 0;

    // Update is called once per frame
    void Update()
    {
        if (balanceofplayers <= -6)
        {
            SceneManager.LoadScene("Lost");
        } else if (balanceofplayers >= 6)
        {
            SceneManager.LoadScene("Won");
        }
    }
}
