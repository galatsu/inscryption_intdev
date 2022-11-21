using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class CardData
{
    public string cardName;
    public Sprite art;
    public int power = 0;
    public int health = 1;
    public int cost = 0;
    public CardEffect[] effects;

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
//card effects not implemented
public abstract class CardEffect
{
    public string effectName = "";
    public Sprite icon;
    public UnityEvent signature;

    void Action()
    {

    }
    /*heres the plan: each cardeffect uses a signature and an action.
    the signature is a game event to listen for, like the start of a turn, or whenever the card is played, or when a card moves into a slot
    the action is then the thing that the card does in response
     */
}
public class MirrorEffect : CardEffect
{
    CardObject target;
    CardObject personal;
    MirrorEffect() 
    { 
        effectName = "Mirror";
        signature.AddListener(Action);
        //signature = when card adjacent changes
    }
    void Action()
    {
        int powerBonus = target.GetPower();
        personal.SetPower(powerBonus+personal.cardAsset.power);
    }
}
public class CandleEffect : CardEffect
{
    CandleEffect()
    {
        effectName = "Candle";
        signature.AddListener(Action);
    }
    void Action()
    {
        //intercept damage taken and add damage-1 back to health
    }
}