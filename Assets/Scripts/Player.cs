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
    public int currentcost = 0;
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
    //state in which we can't select a card; plays when the opponent is doing things
    void CantSelectCard()
    {
        ClearSelection();
    }
    //when it is our turn, we can select a card from our hand
    void CanSelectCardFromHand()
    {
        playerturn = true;
        if (Input.GetMouseButtonDown(0))
        {
            MouseSelect(cam);
        }
        //end your turn; REPLACE WITH A PROPER END TURN BUTTON OR SOMETHING ONCE IMPLEMENTED
        if (Input.inputString == "\b")
        {
            Debug.Log("Moving on");
            playerturn = false;
        }
    }
    //once we've selected our card, we need to pick a slot to play it in; or press space to deselect and pick another
    //maybe add another button to cancel your current selection?
    void MustPlayCardOrCancel()
    {
        if (Input.GetKeyDown("space"))
        {
            ClearSelection();
            Debug.Log("Deselected card; pick a new card");
            stateMachine.ChangeState("CanSelectCardFromHand");
        }
        if (Input.GetMouseButtonDown(0))
        {
            MouseSelect(cam);
        }
        if (cardSelected != null)
        {
            Debug.Log("Card selected");
        }
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
            //clicking a card; transition into the state where we must place it in a slot
            if (objectHit.TryGetComponent(out CardSelectionCollider hitCard))
            {
                //in theory we get the GameObject parent of collider hitCard, in this case the object is the card
                //in practice...cardSelected still isn't anything?
                cardSelected = hitCard.GetParent();
                if (cardSelected == null) { Debug.Log("Please pick a card");  }
                Debug.Log("Selected card; now pick a slot");
                stateMachine.ChangeState("MustPlayCardOrCancel");
            }
            //clicking a slot; if we have a card selected now try to place the card in the slot
            else if (objectHit.TryGetComponent(out SlotSelectionCollider hitSlot))
            {
                slotSelected = hitSlot.GetParent();
                Debug.Log("Selected slot");
            }
        }
        else Debug.Log("No selectable object found");
    }
    //quick way to just clear everything we have selected
    void ClearSelection()
    {
        cardSelected = null;
        slotSelected = null;
    }
    //add cards from our deck to our hand
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
        //if there is a card already in this slot
        if (board.cardSlots[lane, 0].IsOccupied())
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
            board.cardSlots[lane, 0].InsertCard(cardSelected);
            cardSelected = null;
            Debug.Log("Card played in Lane " + lane);
        }

    }
    #endregion
}
