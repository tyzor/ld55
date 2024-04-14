using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardHolder;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
<<<<<<< HEAD
<<<<<<< HEAD
    public HandManager handManager;
    public TextMeshProUGUI resultText;
    public RawImage selectedCardDisplay;
    public TextMeshProUGUI playerPointsText; // Points UI for the player
    public TextMeshProUGUI aiPointsText; // Points UI for the AI
    private Card currentSelectedCard;
    public RawImage largeCardDisplay; // The large central display for the selected card
=======
    public RawImage playerCardDisplay;
    public RawImage aiCardDisplay;
    public TextMeshProUGUI resultText;
=======
    public RawImage playerCardDisplay;
    public RawImage aiCardDisplay;
    public TextMeshProUGUI resultText;
>>>>>>> parent of 2c5962b (war)
    public TextMeshProUGUI playerPointsText;
    public TextMeshProUGUI aiPointsText;
    public RawImage cardBackSprite; // This should be a RawImage with the card back texture already assigned
    public List<Card> playerHand = new List<Card>(); // The player's hand
    public Card selectedCard; // The selected card for battle
<<<<<<< HEAD
>>>>>>> parent of 2c5962b (war)
=======
>>>>>>> parent of 2c5962b (war)

    private Card selectedCard;
    private Card aiCard;
    private int playerPoints = 0;
    private int aiPoints = 0;

    void Start()
    {
<<<<<<< HEAD
        // Initialize game setup if needed
    }

    public void SelectCardForBattle(Card selectedCard)
    {
        currentSelectedCard = selectedCard;
        largeCardDisplay.texture = deckManager.GetCardTexture(selectedCard);
=======
        // Set the AI card back image once
        aiCardDisplay.texture = cardBackSprite.texture;
<<<<<<< HEAD
>>>>>>> parent of 2c5962b (war)
=======
>>>>>>> parent of 2c5962b (war)
    }

    public void Draw()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        // Draw AI card and set it for the battle
        deckManager.DrawAICard();
        aiCard = deckManager.lastAICard; // Ensure this is the card used in Battle method
        // Call HandManager to draw cards to the player's hand
        handManager.DrawCardsToHand(3);
    }

      public void Battle()
    {
        // Use aiCard for the battle instead of deckManager.lastAICard
        if (currentSelectedCard == null || aiCard == null)
        {
            Debug.LogError("Cannot proceed to battle without both cards being selected.");
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
=======
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

=======
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

>>>>>>> parent of 2c5962b (war)
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
<<<<<<< HEAD
>>>>>>> parent of 2c5962b (war)
=======
>>>>>>> parent of 2c5962b (war)

        // Update the points display
        playerPointsText.text = $"Player Points: {playerPoints}";
        aiPointsText.text = $"AI Points: {aiPoints}";

<<<<<<< HEAD
<<<<<<< HEAD
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
=======
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
=======
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
>>>>>>> parent of 2c5962b (war)
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
<<<<<<< HEAD
>>>>>>> parent of 2c5962b (war)
=======
>>>>>>> parent of 2c5962b (war)
    }

    private int GetCardValue(Card card)
    {
        if (card == null)
        {
            Debug.LogError("Card is null. Cannot get card value.");
            return 0;
        }

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

<<<<<<< HEAD

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

    public void UpdateLargeCardDisplay(Card card)
    {
        if (card == null)
        {
            Debug.LogError("Card is null");
            return;
        }

        string imagePath = card.GetCardImagePath();
        Texture2D cardTexture = Resources.Load<Texture2D>($"cards/{imagePath}");
        if (cardTexture != null)
        {
            largeCardDisplay.texture = cardTexture;
        }
        else
        {
            Debug.LogError("Card image not found: " + imagePath);
        }
    }

=======
>>>>>>> parent of 2c5962b (war)
}
