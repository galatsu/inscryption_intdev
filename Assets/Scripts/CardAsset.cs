using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(), System.Serializable()]
public class CardAsset : ScriptableObject
{
    public string cardName;
    public Sprite art;
    public int power = 0;
    public int health = 1;
    public int cost = 0;

    [HideInInspector]
    public UnityEvent cardValidater;
    void OnValidate()
    {
        cardValidater.Invoke();
    }
    //copy constructor
    public CardAsset(CardAsset original)
    {
        cardName = original.cardName;
        art = original.art;
        power = original.power;
        health = original.health;
        cost = original.cost;
    }
    public CardAsset()
    {
        //default constructor
    }
}
