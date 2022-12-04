using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SlotSelectionCollider : MonoBehaviour
{
    Collider2D collider2d;
    CardSlot parentslot;
    private void OnValidate()
    {
        collider2d = GetComponent<Collider2D>();
        parentslot = this.transform.parent.gameObject.GetComponentInParent<CardSlot>();
    }
    public CardSlot GetParent()
    {
        return parentslot;
    }
}
