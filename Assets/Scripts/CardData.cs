using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CardData
{
    public string cardName;
    public Sprite art;
    public int power = 0;
    public int health = 1;
    public int cost = 0;

    public static CardData UnpackFromAsset(CardAsset asset)
    {
        CardData data = new CardData();
        data.art = asset.art;
        data.cardName = asset.cardName;
        data.power = asset.power;
        data.health = asset.health;
        data.cost = asset.cost;

        return data;
    }
    public static CardAsset packIntoAsset(CardData data)
    {
        CardAsset asset = new CardAsset();
        asset.art = data.art;
        asset.cardName = data.cardName;
        asset.power = data.power;
        asset.health = data.health;
        asset.cost = data.cost;

        return asset;
    }
}
