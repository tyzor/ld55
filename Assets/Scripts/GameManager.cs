using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardHolder;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
    public HandManager handManager;
    public TextMeshProUGUI resultText;
    public RawImage selectedCardDisplay;
    public TextMeshProUGUI playerPointsText; // Points UI for the player
    public TextMeshProUGUI aiPointsText; // Points UI for the AI

    private Card selectedCard;
    private int playerPoints = 0;
    private int aiPoints = 0;

    void Start()
    {
        // Initialize game setup if needed
    }

    public void SelectCardForBattle(Card card)
    {
        // Update the selected card data and UI display
        selectedCard = card;
        selectedCardDisplay.texture = deckManager.GetCardTexture(card); // Update the display
    }

    public void Draw()
    {
        // Call HandManager to draw cards to the player's hand
        handManager.DrawCardsToHand(3);
    }

      public void Battle()
    {
        if (selectedCard == null || deckManager.lastAICard == null)
        {
            resultText.text = "Cannot battle without selected card and AI card.";
            return;
        }

        // Reveal AI card and determine the winner
        bool playerWon = DetermineWinner(selectedCard, deckManager.lastAICard);

        // Update points and display the result
        if (playerWon)
        {
            playerPoints++;
            resultText.text = "Player wins!";
            // Additional logic to handle the player winning
        }
        else
        {
            aiPoints++;
            resultText.text = "AI wins!";
            // Additional logic to handle the AI winning
        }

        // Update the points display
        playerPointsText.text = $"Player Points: {playerPoints}";
        aiPointsText.text = $"AI Points: {aiPoints}";

        // Collect cards, handle the end of the battle, etc.
    }

    // This method compares the cards and determines the winner of the battle
    private bool DetermineWinner(Card playerCard, Card aiCard)
    {
        int playerCardValue = GetCardValue(playerCard);
        int aiCardValue = GetCardValue(aiCard);
        // You could have additional logic here if there's a tie (War)
        return playerCardValue > aiCardValue;
    }

    // Call this when there's a tie and "War" needs to happen
    public void War()
    {
        // Placeholder for the "War" mechanic implementation
        Debug.Log("War! Each player draws three more cards.");
        // Implement the mechanics of the "War" scenario here
    }

    private int GetCardValue(Card card)
    {
        Dictionary<string, int> cardValues = new Dictionary<string, int>
        {
            { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 }, { "6", 6 },
            { "7", 7 }, { "8", 8 }, { "9", 9 }, { "10", 10 },
            { "Jack", 11 }, { "Queen", 12 }, { "King", 13 }, { "Ace", 14 }
        };

        if (cardValues.TryGetValue(card.rank, out int value))
        {
            return value;
        }
        else
        {
            Debug.LogError($"Invalid card rank: {card.rank}");
            return 0; // Return 0 or some default value for invalid rank
        }
    }


    private void RevealAICard()
    {
        // Logic to update the AI card display, showing the AI's chosen card
    }

    private void AfterBattleCleanup()
    {
        // Cleanup logic, such as returning non-selected cards to the deck
        // Reset the selected card and any related UI
        selectedCard = null;
        selectedCardDisplay.texture = null; // Set back to default image if you have one
    }

    // Implement additional methods as necessary for your game's logic
    // This may include methods for the "War" mechanic, updating game states, etc.
}
