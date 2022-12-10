using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotRenderer : MonoBehaviour
{
    CardSlot cardSlot;

    void Awake()
    {
        cardSlot = GetComponentInParent<CardSlot>();
    }
    // Update is called once per frame
    void Update()
    {
        int size = cardSlot.slottedCard.Count;
        if (size >= 1)
        {
            for (int i = 0; i < size; i++)
            {
                if (cardSlot.cardInSlot != null)
                {
                    Transform card = cardSlot.slottedCard[i].transform;
                    card.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
                    card.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
                }
            }
        }
    }
}