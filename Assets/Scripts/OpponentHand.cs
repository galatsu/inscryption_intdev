using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHand : MonoBehaviour
{
    public List<CardObject> cards;
    public delegate void CardPlacementConfirmed(bool success, CardObject cardPicked);
    public CardPlacementConfirmed cardPlacementConfirmed;

    public void AddToHand(CardObject cardToAdd)
    {
        //hopefully this captures the value of the card and doesnt get deleted
        cards.Add(cardToAdd);
    }
    public CardPlacementConfirmed TryToPlace()
    {
        cardPlacementConfirmed += RemoveCardConfirmed;
        return cardPlacementConfirmed;
    }
    public void RemoveCardConfirmed(bool success, CardObject cardPicked)
    {
        cardPlacementConfirmed -= RemoveCardConfirmed;
        if (success)
        {
            var card = cardPicked;
            cards.Remove(cardPicked);

        }
    }
}
