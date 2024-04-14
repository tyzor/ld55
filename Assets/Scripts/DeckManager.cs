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
            return drawnCard;
        }
        return null; // Handle this case in your GameManager
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

}
