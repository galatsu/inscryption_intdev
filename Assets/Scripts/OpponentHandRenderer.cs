using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHandRenderer : MonoBehaviour
{
    Hand hand;
    void Awake()
    {
        hand = GetComponentInParent<Hand>();
    }
    // Update is called once per frame
    void Update()
    {
        int size = hand.cards.Count;
        for (int i = 0; i < size; i++)
        {
            Transform card = hand.cards[i].transform;
            card.position = new Vector3(((float)i / (float)size) * 30, 0, i);
        }
    }
}
