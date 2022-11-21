using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CardSelectionCollider : MonoBehaviour
{
    Collider2D collider2d;
    CardObject parent;
    private void OnValidate()
    {
        collider2d = GetComponent<Collider2D>();
        parent = GetComponentInParent<CardObject>();
    }
    public CardObject GetParent()
    {
        return parent;
    }
}
