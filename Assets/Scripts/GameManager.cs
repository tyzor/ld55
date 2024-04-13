using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardHolder;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
    public HandManager handManager;
    public RawImage aiCardDisplay;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI playerPointsText; // Points UI for the player
    public TextMeshProUGUI aiPointsText; // Points UI for the AI
    public Texture cardBackTexture; // Assign a card back texture in the inspector

    private int roundsRemaining = 11;
    private int playerPoints = 0;
    private int aiPoints = 0;
    private Card selectedCard; // The selected card for battle

    void Start()
    {
        // Set the AI card back image once
        aiCardDisplay.texture = cardBackTexture;
        handManager.ClearHandDisplay(); // Ensure the hand is cleared on start
    }

    public void Draw()
    {
        handManager.DrawCardsToHand(3); // Make sure this call is correct and handManager is assigned.
    }

    public void SelectCardForBattle(int cardIndex)
    {
        if (cardIndex < 0 || cardIndex >= handManager.playerHand.Count)
        {
            Debug.LogError("Invalid card index selected.");
            return;
        }

        // Set the selected card from the hand
        selectedCard = handManager.playerHand[cardIndex];
        // Optionally highlight the card or update the UI to reflect selection
        handManager.HighlightCard(selectedCard);
    }

    public void Battle()
    {
        if (selectedCard == null || deckManager.lastAICard == null)
        {
            Debug.LogError("Cannot battle without selected card and AI card.");
            return;
        }

        RevealAICard();
        bool playerWon = DetermineWinner(selectedCard, deckManager.lastAICard);

        if (playerWon)
        {
            playerPoints++;
        }
        else
        {
            aiPoints++;
        }

        playerPointsText.text = $"Player Points: {playerPoints}";
        aiPointsText.text = $"AI Points: {aiPoints}";

        CollectCards(selectedCard, deckManager.lastAICard, playerWon);
        UpdateRoundCount();

        // Clear selected card after battle
        selectedCard = null;
        handManager.ClearHandDisplay();  // Optionally clear the hand display
    }

    private void RevealAICard()
    {
        aiCardDisplay.texture = deckManager.GetCardTexture(deckManager.lastAICard);
    }

    private bool DetermineWinner(Card playerCard, Card aiCard)
    {
        int playerCardValue = GetCardValue(playerCard);
        int aiCardValue = GetCardValue(aiCard);
        resultText.text = playerCardValue > aiCardValue ? "Player wins!" : "AI wins!";
        return playerCardValue > aiCardValue;
    }

    private void CollectCards(Card playerCard, Card aiCard, bool playerWon)
    {
        deckManager.CollectCards(playerCard, aiCard, playerWon);
    }

    private void UpdateRoundCount()
    {
        roundsRemaining--;
        if (roundsRemaining <= 0)
        {
            Debug.Log("New game phase should start now.");
        }
    }

    public void War()
    {
        Debug.Log("Entering WAR!");
        // Implement the WAR mechanics here
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
