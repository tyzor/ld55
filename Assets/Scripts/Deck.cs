using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Audio.SoundFX;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class Deck : MonoBehaviour
{
    private List<CardData> decklist;
    [SerializeField]
    private Card cardPrefab;
    
    public event Action<List<CardData>> DeckChangedAction; 

    public void Init()
    {
         
        Debug.Log($"{name} deck is initializing ");
        decklist = new List<CardData>();    
        
        for(int s=1;s<=5;s++)
        {
            for(int i=1;i<=5;i++)    
            {
                decklist.Add(new CardData(i,(CARDSUIT)s));
            }
        }
        decklist.Shuffle();


        DeckChangedAction?.Invoke(decklist);
    }

    public List<Card> DrawCards(int num) {

        // TODO -- trigger event if we have no draws left

        List<Card> draws = new List<Card>();
        Debug.Log($"Drawing {num} cards from {name}");

        for(var i=0; i<num;i++)
        {
            var cardData = decklist[0];
            decklist.RemoveAt(0);
            DeckChanged();
            Card card = Instantiate<Card>(cardPrefab,transform.parent);
            card.transform.position = GetTopCardPosition();
            card.cardData = cardData;
            draws.Add(card);
            SFX.CARD_FLIP.PlaySoundDelayedRandom(0,0.05f);
        }

        return draws;

    }

    public Vector3 GetTopCardPosition() {

        Vector3 result = Vector3.zero;

        foreach(Transform child in transform)
        {
            if(child.position.y > result.y)
            {
                result = child.position;
            }
        }

        // Offset slightly to avoid z fighting
        return result + Vector3.up * 0.01f;
    }

    public void AddCardsToBottom(Card[] cards)
    {
        foreach(Card card in cards)
        {
            decklist.Add(card.cardData);
            DeckChanged();
        }
    }
    public void AddCardsToBottom(CardData[] cards)
    {
        foreach(CardData card in cards)
        {
            decklist.Add(card);
            DeckChanged();
        }
    }

    public void AddCardToBottom(Card card)
    {
        Card[] cards = { card };
        AddCardsToBottom(cards);
    }

    public bool CanDraw(int numCards)
    {
        return decklist.Count >= numCards;
    }

    private void DeckChanged()
    {
        DeckChangedAction?.Invoke(decklist);
    }

    // Monkey power effect
    public void RandomizeValues(int min=1,int max=10)
    {
        foreach(CardData d in decklist)
        {
            d.value = UnityEngine.Random.Range(min,max+1);
        }
    }

    // Tiger effect
    public void TransmuteCards(int numCards)
    {
        CardData[] cards = GetRandomCards(numCards);
        foreach(CardData card in cards)
        {
            card.suit = (CARDSUIT)UnityEngine.Random.Range(1,6);
        }
    }

    // Rabbit power -- raise all cards below a rank
    public void RaisePower(int minRank, int increase = 1)
    {
        foreach(CardData d in decklist)
        {
            if(d.value <= minRank) d.value = d.value + increase;
        }
    }

    // Pig power -- raise top card
    public void RaiseTopCard(int amount)
    {
        decklist[0].value += amount;
    }

    // Sheep power -- raise random cards by +3
    public void RaiseRandom(int numCards, int amount)
    {
        CardData[] cards = GetRandomCards(numCards);
        foreach(CardData d in cards)
        {
            d.value += amount;
        }
    }

    // Ox power -- raise suit power
    public void RaisePowerSuit(CARDSUIT suit, int amount)
    {
        foreach(CardData card in decklist)
        {
            if(card.suit == suit)
                card.value += amount;
        }
    }


    public CardData[] GetRandomCards(int numCards)
    {
        
        int cardCount = Mathf.Min(numCards,decklist.Count);
        List<int> selection = new List<int>();
        List<CardData> result = new List<CardData>();

        //TODO -- rewrite this to be more efficient
        // probably easier to make an index array, shuffle it and then pull the top n
        for(int i=0;i<cardCount;i++)
        {
            int index;
            do{
                index = UnityEngine.Random.Range(0,decklist.Count);
            } while(selection.Contains(index));
            selection.Add(index);
            result.Add(decklist[index]);
        }
        return result.ToArray();
    }

    // Rat power -- split deck in half round down
    public void SplitDeck()
    {
        int amount = Mathf.FloorToInt(decklist.Count/2f);
        for(int i=0;i<amount;i++)
        {
            decklist.RemoveAt(0);
        }
        DeckChangedAction?.Invoke(decklist);
    }

    // Dragon power
    public void AddPack(CARDSUIT suit, int numCards)
    {
        CardData[] cards = new CardData[numCards];
        for(int n=0;n<numCards;n++)
        {
            CardData card = CardData.RandomCard();
            card.suit = suit;
            cards[n] = card;
        }
        AddCardsToBottom(cards);
    }


}
