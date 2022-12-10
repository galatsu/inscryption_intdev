using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject cardSlotPrefab;
    public CardSlot[,] cardSlots;
    public const int lanes = 4;
    public const int rows = 3;
    public Balance balance;

    [SerializeField]
    AudioSource soundtoplay;
    [SerializeField]
    AudioClip knifeclip;
    [SerializeField]
    AudioClip tearsclip;

    private void Awake()
    {
        AssembleLanes();
    }

    void AssembleLanes()
    {
        cardSlots = new CardSlot[lanes,rows];
        for(int i = 0; i < rows; i++)
        {
            for (int j = 0; j < lanes; j++)
            {
                Debug.Log($"Assembling slot {i}, {j}");
                GameObject slot = Instantiate(cardSlotPrefab, transform);
                //change name of Slot; THIS IS WHERE WE GET LANE AND ROW DATA FROM
                slot.name = "Slot of Lane " + j + " Row " + i;
                slot.transform.position = new Vector3((j * 8) - 12, (i * 9) - 6, 0);

                cardSlots[j, i] = slot.GetComponent<CardSlot>();
            }
        }
    }
    //bool to make sure the opponent can actually play something
    public bool CheckIfLanesAreFull()
    {
        //make an internal bool
        bool hasopenslot = false;
        //iterate through row 2's lanes; if one is open end the loop early, then return true afterwards; else return false
        for (int l = 0; l < lanes; l++)
        {
            if (cardSlots[l, 2].IsOccupied() == false)
            {
                hasopenslot = true;
                break;
            }
        }
        if (hasopenslot) return true;
        else return false;
    }
    public void PlayerAttacks()
    {
        for (int p = 0; p < lanes; p++)
        {
            if (cardSlots[p, 0].IsOccupied() == true)
            {
                CardObject thiscard = cardSlots[p, 0].cardInSlot;
                int thisdamage = thiscard.GetPower();
                Debug.Log("Card " + thiscard.GetName() + " in lane " + p + " is on the attack!");
                if (cardSlots[p, 1].IsOccupied() == true)
                {
                    CardObject enemycard1 = cardSlots[p, 1].cardInSlot;
                    int enemyhealth1 = enemycard1.GetHealth();
                    int resulthealth1 = enemyhealth1 - thisdamage;
                    enemycard1.SetHealth(resulthealth1);
                    //THIS IS WHERE THE KNIFE POWER TAKES PLACE
                    if (resulthealth1 <= 0 && thiscard.GetName() == "the knife")
                    {
                        int increasehealth = thiscard.GetHealth() + 2;
                        int increasedamage = thiscard.GetPower() + 2;
                        thiscard.SetHealth(increasehealth);
                        thiscard.SetPower(increasedamage);
                        soundtoplay.clip = knifeclip;
                        soundtoplay.Play();
                    }
                    cardSlots[p, 1].CheckIfDead();
                }
                else if (cardSlots[p, 2].IsOccupied() == true)
                {
                    CardObject enemycard2 = cardSlots[p, 2].cardInSlot;
                    int enemyhealth2 = enemycard2.GetHealth();
                    int resulthealth2 = enemyhealth2 - thisdamage;
                    enemycard2.SetHealth(resulthealth2);
                    //THIS IS WHERE THE KNIFE POWER TAKES PLACE
                    if (resulthealth2 <= 0 && thiscard.GetName() == "the knife")
                    {
                        int increasehealth = thiscard.GetHealth() + 2;
                        int increasedamage = thiscard.GetPower() + 2;
                        thiscard.SetHealth(increasehealth);
                        thiscard.SetPower(increasedamage);
                        soundtoplay.clip = knifeclip;
                        soundtoplay.Play();
                    }
                    cardSlots[p, 2].CheckIfDead();
                }
                else
                {
                    Debug.Log("Attacking opponent!");
                    balance.balanceofplayers += thisdamage;
                }
            }
        }
    }
    public void OpponentAttacks()
    {
        for (int o = 0; o < lanes; o++)
        {
            if (cardSlots[o, 1].IsOccupied() == true)
            {
                CardObject thatcard = cardSlots[o, 1].cardInSlot;
                int thatdamage = thatcard.GetPower();
                Debug.Log("Card " + thatcard.GetName() + " in lane " + o + " is on the attack!");
                if (cardSlots[o, 0].IsOccupied() == true)
                {
                    CardObject playercard = cardSlots[o, 0].cardInSlot;
                    int playerhealth = playercard.GetHealth();
                    int healthresult = playerhealth - thatdamage;
                    playercard.SetHealth(healthresult);
                    //THIS IS WHERE THE KNIFE POWER TAKES PLACE
                    if (playerhealth <= 0 && thatcard.GetName() == "the knife")
                    {
                        int increasehealth = thatcard.GetHealth() + 2;
                        int increasedamage = thatcard.GetPower() + 2;
                        thatcard.SetHealth(increasehealth);
                        thatcard.SetPower(increasedamage);
                        soundtoplay.clip = knifeclip;
                        soundtoplay.Play();
                    }
                    cardSlots[o, 0].CheckIfDead();
                }
                else
                {
                    Debug.Log("Attacking opponent!");
                    balance.balanceofplayers -= thatdamage;
                }
            }
        }
    }
    public void OpponentsAdvance()
    {
        for (int a = 0; a < lanes; a++)
        {
            CardObject cardhere = cardSlots[a, 2].cardInSlot;
            if (cardSlots[a, 1].IsOccupied() == false && cardhere != null)
            {
                cardSlots[a, 1].InsertCard(cardhere);
                cardSlots[a, 2].cardInSlot = null;
            }
        }
    }
    //THIS IS WHERE THE TEARS POWER GOES
    public void CheckForPlayerTears()
    {
        for (int t = 0; t < lanes; t++)
        {
            bool healing = false;
            CardObject cardhere = cardSlots[t, 0].cardInSlot;
            if (cardSlots[t, 0].IsOccupied() && cardhere.GetName() == "the tears")
            {
                int tl = t - 1;
                int tr = t + 1;
                if (tl >= 0 && cardSlots[tl, 0].IsOccupied())
                {
                    int uphealth = cardSlots[tl, 0].cardInSlot.GetHealth() + 1;
                    cardSlots[tl, 0].cardInSlot.SetHealth(uphealth);
                    if (healing == false) { healing = true; }
                }
                if (tr <= lanes && cardSlots[tr, 0].IsOccupied())
                {
                    int uphealth = cardSlots[tr, 0].cardInSlot.GetHealth() + 1;
                    cardSlots[tr, 0].cardInSlot.SetHealth(uphealth);
                    if (healing == false) { healing = true; }
                }
            }
            if (healing == true) { soundtoplay.clip = tearsclip; soundtoplay.Play(); }
        }
    }
    public void CheckForOpponentTears()
    {
        for (int r = 1; r < rows; r++)
        {
            for (int t = 0; t < lanes; t++)
            {
                bool healing = false;
                CardObject cardhere = cardSlots[t, r].cardInSlot;
                if (cardSlots[t, r].IsOccupied() && cardhere.GetName() == "the tears")
                {
                    int tl = t - 1;
                    int tr = t + 1;
                    if (tl >= 0 && cardSlots[tl, r].IsOccupied())
                    {
                        int uphealth = cardSlots[tl, r].cardInSlot.GetHealth() + 1;
                        cardSlots[tl, 0].cardInSlot.SetHealth(uphealth);
                        if (healing == false) { healing = true; }
                    }
                    if (tr <= lanes && cardSlots[tr, r].IsOccupied())
                    {
                        int uphealth = cardSlots[tr, r].cardInSlot.GetHealth() + 1;
                        cardSlots[tr, 0].cardInSlot.SetHealth(uphealth);
                        if (healing == false) { healing = true; }
                    }
                }
                if (healing == true) { soundtoplay.clip = tearsclip; soundtoplay.Play(); }
            }
        }
    }
}
