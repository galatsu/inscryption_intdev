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

    //for use in DisplayDataText
    public string namecard;
    public int costcard;
    public int damacard;
    public int healcard;

    void AssembleStateMachine()
    {
        stateMachine = new StateMachine(CantSelectCard, CanSelectCardFromHand, MustSacrificeOrCancel, MustPlayCardOrCancel);
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
        if (Input.GetMouseButtonDown(0))
        {
            MouseSelect(cam);
        }
    }
    void MustSacrificeOrCancel()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentcost += slotSelected.cardInSlot.GetCost();
            slotSelected.cardInSlot = null;
            ClearSelection();
            Debug.Log("This card has been sacrificed.");
            stateMachine.ChangeState("CanSelectCardFromHand");
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ClearSelection();
            Debug.Log("Deselected slot; pick a new card");
            stateMachine.ChangeState("CanSelectCardFromHand");
        }
    }
    //once we've selected our card, we need to pick a slot to play it in; or press space to deselect and pick another
    //maybe add another button to cancel your current selection?
    void MustPlayCardOrCancel()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearSelection();
            Debug.Log("Deselected card; pick a new card");
            stateMachine.ChangeState("CanSelectCardFromHand");
        }
        if (Input.GetMouseButtonDown(0))
        {
            MousePick(cam);
        }
    }
    #endregion
    #region actions
    //this one is for use specifically in the "first half" of selection; either to sacrifice a card or to pick a card to play
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
                cardSelected = hitCard.GetParent();
                if (cardSelected == null) { Debug.Log("Please pick a card"); }
                namecard = cardSelected.GetName();
                costcard = cardSelected.GetCost();
                damacard = cardSelected.GetPower();
                healcard = cardSelected.GetHealth();
                Debug.Log("Selected card; now pick a slot");
                stateMachine.ChangeState("MustPlayCardOrCancel");
            }
            //clicking a slot; if we have a card selected now try to place the card in the slot
            else if (objectHit.TryGetComponent(out SlotSelectionCollider hitSlot))
            {
                slotSelected = hitSlot.GetParent();
                int thislane = slotSelected.lane;
                if (slotSelected == null) { Debug.Log("Please pick a slot"); }
                Debug.Log("Selected slot; checking if slot is empty");
                if (slotSelected.IsOccupied()) //if the slot happens to be empty
                {
                    if (slotSelected.row != 0)
                    {
                        Debug.Log("You can only play in the bottommost row.");
                        ClearSelection();
                        stateMachine.ChangeState("CanSelectCardFromHand");
                    }
                    else
                    {
                        Debug.Log("Sacrifice this card?");
                        stateMachine.ChangeState("MustSacrificeOrCancel");
                    }
                }
            }
        }
        else Debug.Log("No selectable object found");
    }
    void MousePick(Camera camera)
    {
        //cast a ray to determine if when we click our mouse is colliding with something
        RaycastHit2D hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        //if we do click something
        if (hit.transform != null)
        {
            Transform objectHit = hit.transform;
            //clicking a slot; now that we have a card selected we now try to place the card in the slot
            if (objectHit.TryGetComponent(out SlotSelectionCollider hitSlot))
            {
                slotSelected = hitSlot.GetParent();
                int thislane = slotSelected.lane;
                if (slotSelected == null) { Debug.Log("Please pick a slot"); }
                Debug.Log("Selected slot; now preparing to place card");
                PlayCardSelectedToBoard(thislane);
            }
        }
        else Debug.Log("No selectable object found");
    }
    //quick way to just clear everything we have selected
    void ClearSelection()
    {
        namecard = "";
        costcard = 0;
        damacard = 0;
        healcard = 0;
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
            ClearSelection();
        }
        //if we dont have enough cost to play the card
        else if (currentcost < cardSelected.GetCost())
        {
            Debug.Log("You don't have enough for this card");
            hand.RemoveCardConfirmed(false, cardSelected);
            ClearSelection();
        }
        //if the lane is empty and we have enough to play the card
        else
        {
            hand.RemoveCardConfirmed(true, cardSelected);
            board.cardSlots[lane, 0].InsertCard(cardSelected);
            Debug.Log("Card played in Lane " + lane);
            currentcost -= cardSelected.GetCost();
            ClearSelection();
            stateMachine.ChangeState("CanSelectCardFromHand");
        }

    }
    #endregion
}
