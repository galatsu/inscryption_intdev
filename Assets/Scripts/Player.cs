using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CardObject cardSelected;
    Board board;
    Hand hand;
    Deck deck;
    StateMachine stateMachine;
    void AssembleStateMachine()
    {
        stateMachine = new StateMachine(CantSelectCard, CanSelectCardFromHand, MustPlayCardOrCancel);
    }
    private void Awake()
    {
        AssembleStateMachine();
    }

    #region states
    void CantSelectCard()
    {

    }
    void CanSelectCardFromHand()
    {

    }
    void MustPlayCardOrCancel()
    {

    }
    #endregion
    #region actions
    void DrawFromDeckToHand()
    {
        var card = deck.Draw();
        hand.AddToHand(card);
    }
    void PickFromHand()
    {
        var confirmer = hand.TryToPlace();
    }
    void PlayCardSelectedToBoard(int lane)
    {
        if (board.cardSlots[lane, 0].IsOccupied())
        {
            hand.RemoveCardConfirmed(false, cardSelected);
            cardSelected = null;
        }
        else
        {
            hand.RemoveCardConfirmed(true, cardSelected);
            board.cardSlots[lane, 0].InsertCard(cardSelected);
            cardSelected = null;
        }

    }
    #endregion
}
