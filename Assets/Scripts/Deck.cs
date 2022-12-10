using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public DefaultDeck defaultDeck;
    public GameObject cardObjectPrefab;
    static List<CardAsset> cards;
    List<CardObject> instanceCards;
    private void Awake()
    {
        InstantiateDeck();
    }
    void InstantiateDeck()
    {
        instanceCards = new List<CardObject>();
        cards = new SerializableDeck(SerializableDeck.Load(defaultDeck.defaultCardAssets)).cards;
        foreach(CardAsset card in cards)
        {
            GameObject cardObject = Instantiate(cardObjectPrefab, transform.position, transform.rotation);
            cardObject.GetComponent<CardObject>().cardAsset = card;
            cardObject.GetComponent<CardObject>().ResetCard();
            instanceCards.Add(cardObject.GetComponent<CardObject>());
        }
        Shuffle();
    }
    public void Shuffle()
    {
        int n = cards.Count;
        System.Random random = new System.Random();
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            CardObject swap = instanceCards[k];
            instanceCards[k] = instanceCards[n];
            instanceCards[n] = swap;
        }
    }
    public CardObject Draw()
    {
        int index = instanceCards.Count - 1;
        Debug.Log($"Drew card at height {index}");
        if (index >= 0)
        {
            CardObject card = instanceCards[index];
            instanceCards.RemoveAt(index);
            return card;
        }
        else return null;
    }
}
[System.Serializable]
public class SerializableDeck
{
    public List<CardAsset> cards = new List<CardAsset>();
    public SerializableDeck(List<CardAsset> cards)
    {
        foreach(CardAsset card in cards)
        {
            this.cards.Add(card);
        }
    }
    //make this safer!!! TODO
    public static void Save(List<CardAsset> cardsToSave)
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/save.deck", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, cardsToSave);
        fs.Close();
    }
    public static List<CardAsset> Load()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/save.deck", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        List<CardAsset> loadedDeck = (List<CardAsset>) formatter.Deserialize(fs);
        fs.Close();
        return loadedDeck;
    }
    public static List<CardAsset> Load(List<CardAsset> returnIfFailed)
    {
        if (File.Exists(Application.persistentDataPath + "/save.deck"))
        {
            FileStream fs = new FileStream(Application.persistentDataPath + "/save.deck", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            List<CardAsset> loadedDeck = (List<CardAsset>)formatter.Deserialize(fs);
            fs.Close();
            return loadedDeck;
        }
        else
        {
            Debug.Log("Failed to load path, returning default cards");
            return returnIfFailed;
        }
            
    }
}