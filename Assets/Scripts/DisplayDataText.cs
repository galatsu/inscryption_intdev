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
        else if (whattoseek == "Desc")
        {
            if (player.namecard == "the mirror")
            {
                TextPro.text = "Add to its Damage that of the opposing creature in the first row.";
            } else if (player.namecard == "the candle")
            {
                TextPro.text = "This can only take 1 damage at a time; when damaged, it retaliates against the attacker.";
            }
            else if (player.namecard == "the knife")
            {
                TextPro.text = "When this kills an opposing creature, this gets +2/+2.";
            }
            else if (player.namecard == "")
            {
                TextPro.text = "";
            }
        }
    }
}
