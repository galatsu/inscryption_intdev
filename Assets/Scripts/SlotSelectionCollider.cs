using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SlotSelectionCollider : MonoBehaviour
{
    Collider2D collider2d;
    CardSlot parent;
    private void OnValidate()
    {
        collider2d = GetComponent<Collider2D>();
        parent = GetComponentInParent<CardSlot>();
    }
    public CardSlot GetParent()
    {
        return parent;
    }
}
