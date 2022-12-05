using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public Player player;

    public void OnButtonPress()
    {
        if (player.playerturn == true)
        {
            player.playerturn = false;
        }
    }
}
