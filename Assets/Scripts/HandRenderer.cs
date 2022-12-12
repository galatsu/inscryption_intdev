using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRenderer : MonoBehaviour
{
    Hand hand;
    float spacingCoefficient = 10;
    public Transform anchor;
    void Awake()
    {
        hand = GetComponentInParent<Hand>();    
    }
    // Update is called once per frame
    void Update()
    {
        int size = hand.cards.Count;
        float leftmostPosition = anchor.position.x - (((size-1)/2f) * spacingCoefficient);
        for(int i = 0; i < size; i++)
        {
            Transform card = hand.cards[i].transform;
            card.position = new Vector3(spacingCoefficient*i + leftmostPosition, -22, i*3);
        }
    }
}
