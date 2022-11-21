using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
                slot.transform.position = new Vector3(j * 7, i * 9, 0);
                //In theory bigger multipliers on i and j should make things spaced out...in practice nothing happens

                cardSlots[j, i] = slot.GetComponent<CardSlot>();
            }
        }
        
    }
}
