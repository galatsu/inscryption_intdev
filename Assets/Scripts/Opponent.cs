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
    public bool CheckIfLanesAreFull()
    {
        bool hasopenslot = false;
        for (int j = 2; j < board.rows; j++)
        {
            for (int k = 0; k < board.lanes; k++)
            {
                if (board.cardSlots[k, j].IsOccupied() == false)
                {
                    hasopenslot = true;
                    break;
                }
            }
        }
        if (hasopenslot) return true;
        else return false;
    }
    void PickAndPlayCard(int lane)
    {
        CardObject cardtoplay = hand.cards[Random.Range(0, hand.cards.Count)];
    }
}
