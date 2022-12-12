using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Balance : MonoBehaviour
{
    public int balanceofplayers = 0;
    public GameObject tendrils;
    public SpriteRenderer tendrilsSprite;
    public Animator tendrilsanim;

    // Update is called once per frame
    void Update()
    {
        Transform balanceicon = this.transform;
        balanceicon.position = new Vector3((balanceofplayers * 8), 20, -10);
        if (balanceofplayers <= -6)
        {
            tendrilsanim.SetBool("hasLost", true);
            SceneManager.LoadScene("Lost");
        } else if (balanceofplayers >= 6)
        {
            SceneManager.LoadScene("Won");
        }
        else if (balanceofplayers >= -5 && balanceofplayers <= -3)
        {
            tendrilsanim.SetBool("isLosing", true);
        }
        else if (balanceofplayers >= -2 && balanceofplayers <= 2)
        {
            tendrilsanim.SetBool("isLosing", false);
        }
    }
}
