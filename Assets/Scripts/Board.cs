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
                slot.transform.position = new Vector3((j * 8) - 12, i * 6, 0);

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
}
