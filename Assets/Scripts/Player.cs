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

    [SerializeField]
    AudioSource soundtoplay;
    [SerializeField]
    AudioClip playclip;
    [SerializeField]
    AudioClip sacriclip;

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
    public string nowprompt;

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
            currentcost += 1;
            //NEED: Is there a way to set slotSelected based on cardSelected within itself?
            int bigsacrifice = cardSelected.GetHealth() - 99;
            cardSelected.SetHealth(bigsacrifice);
            soundtoplay.clip = sacriclip;
            soundtoplay.Play();
            for (int s = 0; s < 4; s++)
            {
                if (board.cardSlots[s, 0].IsOccupied() == true) { board.cardSlots[s, 0].CheckIfDead(); }
            }
            ClearSelection();
            nowprompt = "This card has been sacrificed.";
            stateMachine.ChangeState("CanSelectCardFromHand");
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ClearSelection();
            nowprompt = "Deselected slot; pick a new card";
            stateMachine.ChangeState("CanSelectCardFromHand");
        }
    }
    //once we've selected our card, we need to pick a slot to play it in; or press space to deselect and pick another
    //maybe add another button to cancel your current selection?
    void MustPlayCardOrCancel()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ClearSelection();
            nowprompt = "Deselected card; pick a new card";
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
                //now we see if the card is in the slot; if so, try and set it up for sacrifice
                //with how the cards now appear on the board I don't think the old method is gonna fly
                if (cardSelected.isInSlot == true)
                {
                    if (cardSelected.byPlayer == false)
                    {
                        nowprompt = "You can only play in the bottommost row.";
                        ClearSelection();
                        stateMachine.ChangeState("CanSelectCardFromHand");
                    }
                    else
                    {
                        nowprompt = "Sacrifice the card in this lane? SPACE if yes, BACKSPACE if no.";
                        stateMachine.ChangeState("MustSacrificeOrCancel");
                    }
                }
                else if (cardSelected.isInSlot == false)
                {
                    nowprompt = "Selected card; now pick a slot, or BACKSPACE to deselect";
                    stateMachine.ChangeState("MustPlayCardOrCancel");
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
            if (card != null) { hand.AddToHand(card); card.byPlayer = true; Debug.Log("Added to handtest"); }
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
            nowprompt = "This lane is already full";
            hand.RemoveCardConfirmed(false, cardSelected);
            ClearSelection();
            stateMachine.ChangeState("CanSelectCardFromHand");
        }
        //if we dont have enough cost to play the card
        else if (currentcost < cardSelected.GetCost())
        {
            nowprompt = "You don't have enough for this card";
            hand.RemoveCardConfirmed(false, cardSelected);
            ClearSelection();
            stateMachine.ChangeState("CanSelectCardFromHand");
        }
        //if the lane is empty and we have enough to play the card
        else
        {
            hand.RemoveCardConfirmed(true, cardSelected);
            board.cardSlots[lane, 0].InsertCard(cardSelected);
            nowprompt = "Card played in Lane " + lane;
            cardSelected.isInSlot = true;
            currentcost -= cardSelected.GetCost();
            soundtoplay.clip = playclip;
            soundtoplay.Play();
            ClearSelection();
            stateMachine.ChangeState("CanSelectCardFromHand");
        }

    }
    #endregion
}
