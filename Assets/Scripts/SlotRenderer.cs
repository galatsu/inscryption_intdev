using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotRenderer : MonoBehaviour
{
    CardSlot cardSlot;
    CardObject thiscard;
    public Sprite slotdefault;

    void Awake()
    {
        cardSlot = GetComponentInParent<CardSlot>();
    }
    // Update is called once per frame
    void Update()
    {
        if (cardSlot.IsOccupied() && cardSlot.cardInSlot.GetHealth() > 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = null;
            Transform card = cardSlot.cardInSlot.transform;
            thiscard = cardSlot.cardInSlot;
            card.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            card.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        } //the problem here is getting the card to go away; it really seems like you can just sacrifice the card forever
        //since it apparently doesn't disappear
        else
        {
            Destroy(thiscard);
            this.GetComponent<SpriteRenderer>().sprite = slotdefault;
        }
    }
}