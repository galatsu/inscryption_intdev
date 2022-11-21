using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CardObject cardSelected;
    CardSlot slotSelected;
    Board board;
    Hand hand;
    Deck deck;
    StateMachine stateMachine;
    public Camera cam;
    void AssembleStateMachine()
    {
        stateMachine = new StateMachine(CantSelectCard, CanSelectCardFromHand, MustPlayCardOrCancel);
        stateMachine.ChangeState("CanSelectCardFromHand");
    }
    private void Awake()
    {
        AssembleStateMachine();
    }
    private void Update()
    {
        stateMachine.Execute();
    }
    #region states
    void CantSelectCard()
    {

    }
    void CanSelectCardFromHand()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseSelect(cam);
        }
    }
    void MustPlayCardOrCancel()
    {

    }
    #endregion
    #region actions
    void MouseSelect(Camera camera)
    {
        RaycastHit2D hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.transform != null)
        {
            Transform objectHit = hit.transform;
            if (objectHit.TryGetComponent(out CardSelectionCollider hitCard))
            {
                cardSelected = hitCard.GetParent();
            }
            else if (objectHit.TryGetComponent(out SlotSelectionCollider hitSlot))
            {
                slotSelected = hitSlot.GetParent();
            }
        }
        else Debug.Log("No selectable object found");
    }
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
