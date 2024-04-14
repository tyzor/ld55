using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardVisualData {
    public CARDSUIT suit;
    public Texture frontTexture;
    public Color fontColor;
}

[CreateAssetMenu(menuName = "Card Assets/CardVisualSO", order = 1)]
public class CardVisualDataSO : ScriptableObject
{
    public List<CardVisualData> cardTextures;

    public CardVisualData GetData(CARDSUIT suit)
    {
        foreach(CardVisualData data in cardTextures)
        {
            if(data.suit == suit) return data;
        }
        
        var empty = new CardVisualData();
        empty.suit = CARDSUIT.None;
        return empty;
    }

}
