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
    public void PickAndPlayCard()
    {
        bool success = false;
        List<CardObject> playableCards = new List<CardObject>();
        List<int> playableLanes = new List<int>();
        var confirmer = hand.TryToPlace();
        int costLimit = 0;
        if(hand.cards.Count > 0)
        {
            bool hasALane = false;
            for(int i = 0; i < board.cardSlots.GetLength(0); i++)
            {
                if (!board.cardSlots[i, 2].IsOccupied())
                {
                    hasALane = true;
                    playableLanes.Add(i);
                }
                //while we're iterating over these slots, also update our sacrifice cost limit
                else
                {
                    costLimit++;
                }
            }
            if(hasALane)
            {
                for(int i = 0; i < hand.cards.Count; i++)
                {
                    if (hand.cards[i].GetCost() <= costLimit)
                    {
                        //if we have playable cards in our hand and spaces to play them in, we must play a card
                        success = true;
                        playableCards.Add(hand.cards[i]);
                    }
                }
            }    
        }
        if (!success) confirmer(false, null);
        else
        {
            cardtoplay = playableCards[Random.Range(0, playableCards.Count)];
            lanetoplay = playableLanes[Random.Range(0, playableLanes.Count)];
            confirmer(true, cardtoplay);
            int cost = cardtoplay.GetCost();
            for(int i = 0; i < 4 && cost > 0; i++)
            {
                if (board.cardSlots[i,2].IsOccupied())
                {
                    board.cardSlots[i, 2].TryToRemoveCard()(true, board.cardSlots[i, 2].cardInSlot);
                    cost--;
                }
            }
            board.cardSlots[lanetoplay, 2].InsertCard(cardtoplay);
            Debug.Log("Card played in Lane " + lanetoplay);
        }
        //EVERYTHING BELOW HERE IS OLD - zoey
        //pick a random card from hand; if we don't have enough to play the card, for now just pick something else
        /*
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
        */
    }
}
