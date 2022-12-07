using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CardObject : MonoBehaviour
{
    public CardAsset cardAsset;
    public CardData cardData;
    public SpriteRenderer spriteRenderer;
    public TMP_Text nameText, powerText, healthText;

    /*public static List<MonoBehaviour> commandStack = new List<MonoBehaviour>();*/
    public static UnityEvent onHover = new UnityEvent();
    public static UnityEvent onClick = new UnityEvent();

    //this is for use later in the RandomAssetPicker
    string[] cardNames = new string[] {"MirrorCard", "MirrorCard", "MirrorCard", "MirrorCard", "CandleCard", "CandleCard",
        "ProphetCard", "EyesCard", "EyesCard", "TendrilsCard", "TendrilsCard", "KnifeCard"};

    public CardAsset mirrorAsset;
    public CardAsset candleAsset;
    public CardAsset eyesAsset;
    public CardAsset prophetAsset;
    public CardAsset knifeAsset;
    public CardAsset tendrilsAsset;

    void OnHover()
    {
        onHover.Invoke();
    }

    void OnClick()
    {
       onClick.Invoke();
    }

    void OnValidate()
    {
        //Card object in-game has to know when the card assets are updated
        cardAsset.cardValidater.AddListener(ResetCard);
        ResetCard();
    }
    void Awake()
    {
        RandomAssetPicker();
        ResetCard();
    }
    void ResetCard()
    {
        LoadCardDataFromAsset();

        spriteRenderer.sprite = cardData.art;
        nameText.text = cardData.cardName;
        powerText.text = cardData.power.ToString();
        healthText.text = cardData.health.ToString();
    }
    //update card is meant to be used in-game. it does NOT reload the card data from the asset but is otherwise the same as reset card
    void UpdateCard()
    {
        spriteRenderer.sprite = cardData.art;
        nameText.text = cardData.cardName;
        powerText.text = cardData.power.ToString();
        healthText.text = cardData.health.ToString();

    }
    void LoadCardDataFromAsset()
    {
        cardData = CardData.UnpackFromAsset(cardAsset);
    }
    //this randomly selects a card from the list of string names, mentioned above, then sets the cardAsset accordingly
    void RandomAssetPicker()
    {
        string selectedcard = cardNames[Random.Range(0, cardNames.Length)];
        if (selectedcard == "MirrorCard") { cardAsset = mirrorAsset;  }
        else if (selectedcard == "CandleCard") { cardAsset = candleAsset; }
        else if (selectedcard == "EyesCard") { cardAsset = eyesAsset; }
        else if (selectedcard == "ProphetCard") { cardAsset = prophetAsset; }
        else if (selectedcard == "KnifeCard") { cardAsset = knifeAsset; }
        else if (selectedcard == "TendrilsCard") { cardAsset = tendrilsAsset; }
    }

    #region card data getters and setters
    public int GetHealth()
    {
        return cardData.health;
    }
    public int GetPower()
    {
        return cardData.power;
    }
    public int GetCost()
    {
        return cardData.cost;
    }
    public string GetName()
    {
        return cardData.cardName;
    }
    public Sprite GetArt()
    {
        return cardData.art;
    }
    public void SetHealth(int health)
    {
        cardData.health = health;
    }
    public void SetPower(int power)
    {
        cardData.power = power;
    }
    public void SetCost(int cost) 
    { 
        cardData.cost = cost;
    }
    public void SetName(string name)
    {
        cardData.cardName = name;
    }
    #endregion
}

