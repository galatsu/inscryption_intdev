using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardSlot : MonoBehaviour
{
    public CardObject cardInSlot;
    private Board board;
    public delegate CardObject CardPlacementConfirmed(bool success);
    public CardPlacementConfirmed cardPlacementConfirmed;
    public int lane;
    public int row;
    //if there is nothing in the slot return false meaning empty
    void Start()
    {
        string tolane = gameObject.name.Substring(13, 1);
        string torow = gameObject.name.Substring(19, 1);
        lane = int.Parse(tolane);
        row = int.Parse(torow);
    }
    public bool IsOccupied()
    {
        if (cardInSlot == null) return false;
        else return true;
    }
    //if the slot is empty make the card enter the slot, or any other application that requires an empty/non-empty slot
    public void InsertCard(CardObject card)
    {
        if (!IsOccupied())
        {
            cardInSlot = card;
        }
    }
    public CardPlacementConfirmed TryToRemoveCard()
    {
        cardPlacementConfirmed+=RemoveCardConfirmed;
        return cardPlacementConfirmed;
    }
    CardObject RemoveCardConfirmed(bool success)
    {
        cardPlacementConfirmed -= RemoveCardConfirmed;
        if (success) {
            var card = cardInSlot;
            cardInSlot = null;
            return card;
        }
        else
        {
            return null;
        }
        
    }
    //if the card's health is below zero, the card gets removed
    public void CheckIfDead()
    {
        if (cardInSlot.GetHealth() <= 0)
        {
            Debug.Log("A card has been defeated; card " + cardInSlot.GetName());
            cardInSlot = null;
        }
    }
}
