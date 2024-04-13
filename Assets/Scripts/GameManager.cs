using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardHolder;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
    public RawImage playerCardDisplay;
    public RawImage aiCardDisplay;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI playerPointsText;
    public TextMeshProUGUI aiPointsText;
    public RawImage cardBackSprite; // This should be a RawImage with the card back texture already assigned
    public List<Card> playerHand = new List<Card>(); // The player's hand
    public Card selectedCard; // The selected card for battle

    private int roundsRemaining = 11;
    private int playerPoints = 0;
    private int aiPoints = 0;

    void Start()
    {
        // Set the AI card back image once
        aiCardDisplay.texture = cardBackSprite.texture;
    }

    public void Draw()
    {
        // Draw 3 cards to the player's hand
        for (int i = 0; i < 3; i++)
        {
            if (deckManager.playerDeck.Count > 0)
            {
                Card card = deckManager.DrawCard();
                playerHand.Add(card);
                // Assuming you have a method to update a visual list or hand display
                UpdateCardDisplayInHand(card); // Implement this to visually add card to UI hand
            }
        }
        // Draw AI card and update lastAICard but don't show it
        deckManager.DrawAICard();
    }

    // This method should be called when a player selects a card from the hand
    public void SelectCardForBattle(Card card)
    {
        selectedCard = card;
        playerCardDisplay.texture = deckManager.GetCardTexture(selectedCard);
        // Optionally remove the card from hand and update UI
        playerHand.Remove(selectedCard);
    }

    private void UpdateCardDisplayInHand(Card card)
    {
        // Assuming you have some form of UI representation for cards in hand
        // This method should create and display card UI elements in the player's hand area
        Debug.Log("Display card in hand: " + card.rank + " of " + card.suit);
        // Implement UI update logic here
    }

    public void Battle()
    {
        if (selectedCard == null || deckManager.lastAICard == null)
        {
            Debug.LogError("Cannot battle without both cards drawn and selected.");
            return;
        }

        RevealAICard();
        bool playerWon = DetermineWinner(selectedCard, deckManager.lastAICard);

        if (playerWon) playerPoints++;
        else aiPoints++;

        playerPointsText.text = $"Player Points: {playerPoints}";
        aiPointsText.text = $"AI Points: {aiPoints}";

        CollectCards(selectedCard, deckManager.lastAICard, playerWon);
        UpdateRoundCount();
        // Reset selectedCard for next round
        selectedCard = null;
    }

    private void RevealAICard()
    {
        // Reveal the AI card only during battle
        aiCardDisplay.texture = deckManager.GetCardTexture(deckManager.lastAICard);
    }


    private bool DetermineWinner(Card playerCard, Card aiCard)
    {
        int playerCardValue = deckManager.GetCardValue(playerCard);
        int aiCardValue = deckManager.GetCardValue(aiCard);
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
        // Check if it's time for the new mechanic after 11 battles
        if (roundsRemaining <= 0)
        {
            // Implement the new mechanic here
        }
    }

    public void War()
    {
        // Placeholder for the WAR mechanic implementation
        Debug.Log("Entering WAR!");
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
