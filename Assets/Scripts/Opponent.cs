using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    //This builds on the player class but is very simple since it doesn't require player input
    CardObject cardSelected;
    CardSlot slotSelected;
    [SerializeField]
    Board board;
    [SerializeField]
    public OpponentHand hand;
    [SerializeField]
    Deck deck;
    public StateMachine stateMachine;
    public int currentcost = 0;
    public int numcards = 4;
    CardObject cardtoplay;
    int lanetoplay;

    public void DrawFromDeckToHand(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            var card = deck.Draw();
            if (numcards < 4) { numcards++; }
            if (card != null) { hand.AddToHand(card); Debug.Log("Added to handtest"); }
        }
    }
    void PickFromHand()
    {
        var confirmer = hand.TryToPlace();
    }
    public void PickAndPlayCard()
    {
        bool hasacard = false;
        bool hasalane = false;
        //pick a random card from hand; if we don't have enough to play the card, for now just pick something else
        while (!hasacard)
        {
            cardtoplay = hand.cards[Random.Range(0, hand.cards.Count)];
            if (currentcost < cardtoplay.GetCost())
            {
                hasacard = false;
            } else
            {
                hasacard = true;
            }
        }
        //pick a random lane to play the card in; again if the lane is occupied pick something else
        while (!hasalane)
        {
            lanetoplay = Random.Range(0, 3);
            if (board.cardSlots[lanetoplay, 2].IsOccupied())
            {
                hasalane = false;
            }
            else
            {
                hasalane = true;
            }
        }
        if (hasacard && hasalane)
        {
            hand.RemoveCardConfirmed(true, cardtoplay);
            board.cardSlots[lanetoplay, 2].InsertCard(cardtoplay);
            Debug.Log("Card played in Lane " + lanetoplay);
            currentcost -= cardtoplay.GetCost();
        }
    }
}
