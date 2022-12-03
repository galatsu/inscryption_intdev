using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CardObject cardSelected;
    CardSlot slotSelected;
    [SerializeField]
    Board board;
    [SerializeField]
    public Hand hand;
    [SerializeField]
    Deck deck;
    public StateMachine stateMachine;
    public Camera cam;
    public int numcards = 4;
    public bool playerturn = false;
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
        this.stateMachine.Execute();
    }
    #region states
    void CantSelectCard()
    {
        ClearSelection();
    }
    void CanSelectCardFromHand()
    {
        playerturn = true;
        if (Input.GetMouseButtonDown(0))
        {
            MouseSelect(cam);
        }
        if (Input.inputString == "\b")
        {
            Debug.Log("Moving on");
            playerturn = false;
        }
    }
    void MustPlayCardOrCancel()
    {
        
    }
    #endregion
    #region actions
    void MouseSelect(Camera camera)
    {
        //cast a ray to determine if when we click our mouse is colliding with something
        RaycastHit2D hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        //if we do click something
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
    void ClearSelection()
    {
        cardSelected = null;
        slotSelected = null;
    }
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
