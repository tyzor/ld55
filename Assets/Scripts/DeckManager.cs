using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Card_holder;
using System.Globalization;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public List<Card> playerDeck = new List<Card>();
    public List<Card> aiDeck = new List<Card>();
    public RawImage cardDisplay;
    public Card lastAICard;
    public Card lastPlayerCard;


    void Start()
    {
        PopulateDeck(playerDeck);
        PopulateDeck(aiDeck);
        ShuffleDeck(playerDeck);
        ShuffleDeck(aiDeck);
    }

    void PopulateDeck(List<Card> deck)
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                deck.Add(new Card(rank, suit));
            }
        }
    }

    public void ShuffleDeck(List<Card> deck)
    {
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }

    public void DrawCard()
    {
        if (playerDeck.Count > 0)
        {
            Card drawnCard = playerDeck[0];
            playerDeck.RemoveAt(0);
            UpdateCardDisplay(drawnCard);
            Debug.Log("Drawn Card: " + drawnCard.rank + " of " + drawnCard.suit);
        }
        else
        {
            Debug.Log("No more cards in the deck.");
        }
    }

    public void DrawAICard()
    {
        if (aiDeck.Count > 0)
        {
            lastAICard = aiDeck[0];
            aiDeck.RemoveAt(0);
            // Note: Do not display this card as it is supposed to be face down.
        }
        else
        {
            Debug.Log("AI deck is empty.");
        }
    }


    public void UpdateCardDisplay(Card card)
    {
        string imageFileName = card.rank.ToLower() + "_of_" + card.suit.ToLower();
        Texture2D cardImage = Resources.Load<Texture2D>("cards/" + imageFileName);
        if (cardImage != null)
        {
            cardDisplay.texture = cardImage;
        }
        else
        {
            Debug.LogWarning("Image not found: " + imageFileName);
        }
    }


}
