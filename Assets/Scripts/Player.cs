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
    public string fulldata;

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
            soundtoplay.clip = sacriclip;
            soundtoplay.Play();
            //THIS IS WHERE THE MANY EYES POWER TAKES PLACE
            if (cardSelected.GetName() == "the many eyes")
            {
                int eyesacrifice = cardSelected.GetHealth() - 1;
                cardSelected.SetHealth(eyesacrifice);
            }
            else
            {
                int bigsacrifice = cardSelected.GetHealth() - 99;
                cardSelected.SetHealth(bigsacrifice);
            }
            if (cardSelected.DeadCard() == true || slotSelected.CheckIfDead() == true)
            {
                slotSelected.RemoveCardConfirmed(true, cardSelected);
            } else
            {
                slotSelected.RemoveCardConfirmed(false, cardSelected);
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
                fulldata = costcard.ToString() + "-cost " + healcard.ToString() + "-health/" + damacard.ToString() + "-damage";
                //now we see if the card is in the slot; if so, try and set it up for sacrifice
                //with how the cards now appear on the board I don't think the old method is gonna fly
                if (cardSelected.isInSlot == true)
                {
                    if (cardSelected.byPlayer == false) //if the card is not owned by the player on the board; i.e.
                        //if a card in a deck or on the opponent's side is clicked
                    {
                        nowprompt = "You can only play in the bottommost row.";
                        ClearSelection();
                        stateMachine.ChangeState("CanSelectCardFromHand");
                    }
                    else
                    {
                        Transform positioncard = cardSelected.transform;
                        float thislane = (positioncard.position.x + 12.0f) / 8.0f;
                        int lanepos = (int)thislane;
                        slotSelected = board.cardSlots[lanepos, 0];
                        nowprompt = "Sacrifice the card in this lane? SPACE if yes, BACKSPACE if no.";
                        Debug.Log("Slot of Lane " + lanepos + " Row " + 0);
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
        fulldata = "";
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
