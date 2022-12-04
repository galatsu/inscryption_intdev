using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    //This builds on the player class but is very simple since it doesn't require player input
    CardObject cardSelected;
    CardSlot slotSelected;
    [SerializeField]
    Board board;
    [SerializeField]
    public Hand hand;
    [SerializeField]
    Deck deck;
    public StateMachine stateMachine;
    public int currentcost = 0;
    public int numcards = 4;

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
    void PlayCardSelectedToBoard(int lane)
    {
        whichcard = Random.Range(0, numcards);
        cardSelected = hand.cards[whichcard];
        //if there is a card already in this slot
        if (board.cardSlots[lane, 2].IsOccupied())
        {
            Debug.Log("This lane is already full");
            hand.RemoveCardConfirmed(false, cardSelected);
            cardSelected = null;
        }
        //if we dont have enough cost to play the card
        if (currentcost < cardSelected.cardData.cost)
        {
            Debug.Log("You don't have enough for this card");
            hand.RemoveCardConfirmed(false, cardSelected);
            cardSelected = null;
        }
        //if the lane is empty and we have enough to play the card
        else
        {
            hand.RemoveCardConfirmed(true, cardSelected);
            board.cardSlots[lane, 2].InsertCard(cardSelected);
            cardSelected = null;
            Debug.Log("Card played in Lane " + lane);
        }

    }
# endr
}
