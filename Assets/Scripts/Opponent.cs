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
    Deck deck;
    public StateMachine stateMachine;
    public int currentcost = 0;

    public void PlayEnemyCard(int lane)
    {
        var card = deck.Draw();
        board.cardSlots[lane, 0].InsertCard(card);
    }
}
