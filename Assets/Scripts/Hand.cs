using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    List<CardObject> cards;
    public delegate CardObject CardPlacementConfirmed(bool success, CardObject cardPicked);
    public CardPlacementConfirmed cardPlacementConfirmed;
    public Deck deck;

    void AddToHand(CardObject cardToAdd)
    {
        //hopefully this captures the value of the card and doesnt get deleted
        cards.Add(cardToAdd);
    }
    public CardPlacementConfirmed TryToPlace()
    {
        cardPlacementConfirmed += RemoveCardConfirmed;
        return cardPlacementConfirmed;
    }
    CardObject RemoveCardConfirmed(bool success, CardObject cardPicked)
    {
        cardPlacementConfirmed -= RemoveCardConfirmed;
        if (success)
        {
            var card = cardPicked;
            cards.Remove(cardPicked);
            return card;
        }
        else
        {
            return null;
        }
    }
}
