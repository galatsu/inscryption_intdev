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
    public Hand hand;
    [SerializeField]
    Deck deck;
    public StateMachine stateMachine;
    public int numcards = 4;
    public void DrawFromDeckToHand(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            var card = deck.Draw();
            numcards++;
            if (card != null) { hand.AddToHand(card); Debug.Log("Added to handtest"); }
        }
    }
}
