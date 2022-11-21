using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CardObject : MonoBehaviour
{
    public CardAsset cardAsset;
    public SpriteRenderer spriteRenderer;
    public TMP_Text nameText, powerText, healthText;

    /*public static List<MonoBehaviour> commandStack = new List<MonoBehaviour>();
    public static UnityEvent onHover = new UnityEvent();
    public static UnityEvent onClick = new UnityEvent();*/
    void OnHover()
    {
        //onHover.Invoke();
    }

    void OnClick()
    {
       //onClick.Invoke();
    }

    void OnValidate()
    {
        //Card object in-game has to know when the card assets are updated
        cardAsset.cardValidater.AddListener(UpdateCard);
        UpdateCard();
    }
    void Start()
    {
        UpdateCard();
    }
    void UpdateCard()
    {
        spriteRenderer.sprite = cardAsset.art;
        nameText.text = cardAsset.cardName;
        powerText.text = cardAsset.power.ToString();
        healthText.text = cardAsset.health.ToString();
    }
}

