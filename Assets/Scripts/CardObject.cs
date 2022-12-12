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
    public bool isInSlot = false;
    public bool byPlayer = false;

    /*public static List<MonoBehaviour> commandStack = new List<MonoBehaviour>();*/
    public static UnityEvent onHover = new UnityEvent();
    public static UnityEvent onClick = new UnityEvent();

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
        ResetCard();
    }
    public void ResetCard()
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
    public bool DeadCard()
    {
        if (GetHealth() == 0) return true;
        else return false;
    }
    public void LeaveAndDie()
    {
        transform.position = new Vector3(60, 40, 0);
        Destroy(gameObject);
    }
}

