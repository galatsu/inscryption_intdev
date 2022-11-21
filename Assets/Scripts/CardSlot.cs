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
    public bool IsOccupied()
    {
        if (cardInSlot == null) return false;
        else return true;
    }
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
