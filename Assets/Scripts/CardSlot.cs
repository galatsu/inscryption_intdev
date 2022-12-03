using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardSlot : MonoBehaviour
{
    CardObject cardInSlot;
    private Board board;
    public delegate CardObject CardPlacementConfirmed(bool success);
    public CardPlacementConfirmed cardPlacementConfirmed;
    public int lane;
    public int row;
    //if there is nothing in the slot return false meaning empty
    public bool IsOccupied()
    {
        if (cardInSlot == null) return false;
        else return true;
    }
    //if the slot is empty make the card enter the slot
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
}
