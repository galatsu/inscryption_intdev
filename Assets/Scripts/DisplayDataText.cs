using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDataText : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI TextPro;

    void Update()
    {
        TextPro.text = player.currentcost.ToString();
    }
}
