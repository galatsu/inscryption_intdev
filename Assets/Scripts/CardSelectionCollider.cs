using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CardSelectionCollider : MonoBehaviour
{
    Collider2D collider2d;
    CardObject parentcard;
    private void OnValidate()
    {
        collider2d = GetComponent<Collider2D>();
        parentcard = transform.parent.GetComponent<CardObject>();
    }
    public CardObject GetParent()
    {
        return parentcard;
    }
}
