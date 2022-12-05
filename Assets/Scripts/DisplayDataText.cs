using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDataText : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI TextPro;
    public string whattoseek;

    void Update()
    {
        if (whattoseek == "Cost") { TextPro.text = player.currentcost.ToString(); }
        else if (whattoseek == "Name") { TextPro.text = player.namecard; }
        else if (whattoseek == "Stats")
        {
            TextPro.text = player.costcard.ToString() + "-cost " + player.damacard.ToString() + "-damage " + player.healcard.ToString() + "-health";
        } else if (whattoseek == "Prompt") { TextPro.text = player.nowprompt; }
    }
}
