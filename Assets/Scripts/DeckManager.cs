using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardHolder;
using System.Globalization;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public List<Card> playerDeck = new List<Card>();
    public List<Card> aiDeck = new List<Card>();
    public RawImage cardDisplay;
    public Card lastAICard;
    public Card lastPlayerCard;
    public RawImage cardBackRawImage;
    public List<Texture2D> cardBackTextures;


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

    public Card DrawCard()
    {
        if (playerDeck.Count > 0)
        {
            Card drawnCard = playerDeck[0];
            playerDeck.RemoveAt(0);
            lastPlayerCard = drawnCard;
            Debug.Log("Drawn Card: " + drawnCard.rank + " of " + drawnCard.suit);
            return drawnCard;
        }
        return null; // Handle this case in your GameManager
    }

    public Card DrawAICard()
    {
        if (aiDeck.Count > 0)
        {
            Card drawnCard = aiDeck[0];
            aiDeck.RemoveAt(0);
            lastAICard = drawnCard;
            cardBackRawImage.texture = GetBackTexture(drawnCard);
            return drawnCard;
        }
        return null; // Handle this case in your GameManager
    }

    public Texture2D GetBackTexture(Card card)
    {
        if (card == null || cardBackTextures == null || cardBackTextures.Count == 0)
            return null;

        int index = GetCardIndex(card);
        if (index < 0 || index >= cardBackTextures.Count)
            return null;

        return cardBackTextures[index];
    }

    public Texture2D GetCardTexture(Card card)
    {
        if (card == null) return null;
        string path = "cards/" + card.rank + "_of_" + card.suit;
        return Resources.Load<Texture2D>(path);
    }

    public int GetCardValue(Card card)
    {
        if (card == null)
        {
            Debug.LogError("Attempted to get value of a null card.");
            return 0;  // Default or error value
        }

        Dictionary<string, int> cardValues = new Dictionary<string, int>
        {
            ["2"] = 2, ["3"] = 3, ["4"] = 4, ["5"] = 5, ["6"] = 6,
            ["7"] = 7, ["8"] = 8, ["9"] = 9, ["10"] = 10,
            ["Jack"] = 11, ["Queen"] = 12, ["King"] = 13, ["Ace"] = 14
        };

        if (cardValues.TryGetValue(card.rank, out var value))
            return value;
        else
        {
            Debug.LogError($"Invalid card rank: {card.rank}");
            return 0;  // Handle unexpected card rank
        }
    }

    private int GetCardIndex(Card card)
    {
        // Assuming ranks are numbered from 0
        int rankIndex;
        if (int.TryParse(card.rank, out rankIndex)) // For numbered cards
        {
            return rankIndex - 2; // for "2" as the first card rank with an index of 0
        }
        else // For face cards
        {
            switch (card.rank)
            {
                case "Jack": return 9;
                case "Queen": return 10;
                case "King": return 11;
                case "Ace": return 12;
                default: return -1; // Unknown rank
            }
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

    public void CollectCards(Card winnerCard, Card loserCard, bool playerWins)
    {
        if (playerWins)
        {
            playerDeck.Add(winnerCard);
            playerDeck.Add(loserCard);
        }
        else
        {
            aiDeck.Add(winnerCard);
            aiDeck.Add(loserCard);
        }
    }

    public void IncreaseSuitValue(int increment)
    {
        // Go through each deck and increment the value of cards from a specific suit
    }

    public void ConvertRank(string action)
    {
        // Convert all cards of a specific rank to a higher rank
    }

    public void ReplaceAllCardsWithRandomValues()
    {
        // Replace all cards in player and AI decks with cards of random ranks and suits
    }

    public void ReplaceAICardWithLowerRank()
    {
        // Replace the last drawn AI card with a card of lower rank if possible
    }

    public void CutDecksInHalf()
    {
        // Reduce the size of both the player's and AI's decks by half
    }


}