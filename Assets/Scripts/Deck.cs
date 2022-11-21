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
    void InstantiateDeck()
    {
        instanceCards = new List<CardObject>();
        cards = new SerializableDeck(SerializableDeck.Load(defaultDeck.defaultCardAssets)).cards;
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
            cards.Add(new CardAsset(card));
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
            return returnIfFailed;
        }
            
    }
}