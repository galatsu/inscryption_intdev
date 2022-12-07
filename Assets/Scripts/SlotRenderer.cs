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
        if (cardSlot.IsOccupied())
        {
            Transform card = cardSlot.cardInSlot.transform;
            card.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            card.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
    }
}