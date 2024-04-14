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
    public RawImage playerCardDisplay;
    public RawImage aiCardDisplay;
    public int playerEnergy = 0; // Current energy of the player
    public int energyGain = 1; // Amount of energy gained per specified event
    public int summonCost = 10; // Energy cost to activate a summon effect
    

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
        //selectedCard = card;
//        selectedCardDisplay.texture = deckManager.GetCardTexture(card); // Update the display

        selectedCard = card;
        playerCardDisplay.texture = deckManager.GetCardTexture(selectedCard);
    }

    public void Draw()
    {
        // Call HandManager to draw cards to the player's hand
        handManager.DrawCardsToHand(3);
        deckManager.DrawAICard();
    }

      public void Battle()
    {
        if (selectedCard == null || deckManager.lastAICard == null)
        {
            resultText.text = "Cannot battle without selected card and AI card.";
            return;
        }

        // Reveal AI card and determine the winner
        RevealAICard();
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
        IncrementEnergy();
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
        // Reveal the AI card only during battle
        aiCardDisplay.texture = deckManager.GetCardTexture(deckManager.lastAICard);
    }

    private void AfterBattleCleanup()
    {
        // Cleanup logic, such as returning non-selected cards to the deck
        // Reset the selected card and any related UI
        selectedCard = null;
        selectedCardDisplay.texture = null; // Set back to default image if you have one
        CheckEnergyAndSummon();
    }

    public void IncrementEnergy()
    {
        playerEnergy += energyGain;
        CheckEnergyAndSummon(); // Check if summon can be activated after energy increment
    }

    void CheckEnergyAndSummon()
    {
        if (playerEnergy >= summonCost)
        {
            playerEnergy -= summonCost; // Deduct the energy cost
            ActivateSummonEffect(SelectSummonEffect()); // Activate an effect based on some selection logic
        }
    }

    public void SummonMenu()
    {
        ActivateSummonEffect(1);
    }

    public void ActivateSummonEffect(int effectId)
    {
        switch (effectId)
        {
            case 1: // Horse
                handManager.DrawCardsToHand(1);
                break;
            case 2: // Dragon
                deckManager.IncreaseSuitValue(1); // Implement this method in DeckManager
                break;
            case 3: // Rabbit
                deckManager.ConvertRank("increase"); // Implement this method in DeckManager
                break;
            case 4: // Monkey
                deckManager.ShuffleDeck(deckManager.aiDeck);
                deckManager.ReplaceAllCardsWithRandomValues(); // Implement in DeckManager
                break;
            case 6: // Rooster
                // Implement double energy gains from winning, use a counter to track turns
                break;
            case 7: // Snake
                // Implement double energy gains from losing, use a counter to track turns
                break;
            case 8: // Ox
                // Automatically win the next battle
                break;
            case 9: // Tiger
                // Implement functionality to see opponent's cards
                break;
            case 10: // Dog
                // Reduce the next summon energy requirement by half
                break;
            case 11: // Sheep
                deckManager.ReplaceAICardWithLowerRank(); // Implement this in DeckManager
                break;
            case 12: // Rat
                deckManager.CutDecksInHalf(); // Implement this in DeckManager
                break;
            default:
                Debug.LogError("Summon effect not recognized.");
                break;
        }
    }

}