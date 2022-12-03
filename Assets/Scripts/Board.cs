using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject cardSlotPrefab;
    public CardSlot[,] cardSlots;
    const int lanes = 4;
    const int rows = 3;

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
                //for convenience sake change name of slot to show lane and row
                slot.name = "Slot of Lane " + j + " Row " + i;
                cardSlotPrefab.lane = j;
                cardSlotPrefab.row = i;
                slot.transform.position = new Vector3(j * 7, i * 8, 0);

                cardSlots[j, i] = slot.GetComponent<CardSlot>();
            }
        }
        
    }
}
