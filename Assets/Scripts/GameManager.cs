using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Card_holder;
using System.Globalization;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
    public RawImage aiCardDisplay;
    public TextMeshProUGUI resultText;
    
    public void Battle()
    {
        // Ensure both the player and AI have drawn a card
        //deckManager.DrawCard();
        deckManager.DrawAICard();

        // Reveal AI's card
        UpdateCardDisplay(deckManager.lastAICard, aiCardDisplay);

        // Compare the cards and determine the winner
        string result = DetermineWinner(deckManager.lastPlayerCard, deckManager.lastAICard);
        resultText.text = result;
    }

    void UpdateCardDisplay(Card card, RawImage display)
    {
        string imageFileName = card.rank.ToLower() + "_of_" + card.suit.ToLower();
        Texture2D cardImage = Resources.Load<Texture2D>("cards/" + imageFileName);
        if (cardImage != null)
        {
            display.texture = cardImage;
        }
        else
        {
            Debug.LogError("Image not found: " + imageFileName);
        }
    }

    string DetermineWinner(Card playerCard, Card aiCard)
    {
        // Implement your card comparison logic here to determine the winner
        // For simplicity, let's say higher rank wins
        // You would need to implement more comprehensive comparison based on card game rules

        int playerValue = GetCardValue(playerCard);
        int aiValue = GetCardValue(aiCard);

        if (playerValue > aiValue)
            return "Player wins!";
        else if (aiValue > playerValue)
            return "AI wins!";
        else
            return "It's a tie!";
    }

    int GetCardValue(Card card)
    {
        if (card == null)
        {
            Debug.LogError("Card is null");
            return 0;
        }

        Dictionary<string, int> rankValues = new Dictionary<string, int>()
        {
            {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6},
            {"7", 7}, {"8", 8}, {"9", 9}, {"10", 10},
            {"Jack", 11}, {"Queen", 12}, {"King", 13}, {"Ace", 14}
        };

        if (rankValues.TryGetValue(card.rank, out int value))
        {
            return value;
        }
        else
        {
            Debug.LogError($"Invalid card rank: {card.rank}");
            return 0; // Invalid rank case
        }
    }

}
