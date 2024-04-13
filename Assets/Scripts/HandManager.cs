using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardHolder;
using System.Globalization;
using TMPro;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject handContainer; // Parent GameObject for cards
    public List<Card> playerHand = new List<Card>();
    public Card selectedCard; // The card selected for battle

    public void DrawCardsToHand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (deckManager.playerDeck.Count > 0)
            {
                Card drawnCard = deckManager.DrawCard();
                playerHand.Add(drawnCard);
                AddCardToHandUI(drawnCard);
            }
        }
    }

    private Sprite GetCardSprite(Card card)
    {
        // Load the sprite for the card
        string path = "Cards/" + card.rank.ToLower() + "_of_" + card.suit.ToLower();
        return Resources.Load<Sprite>(path);
    }


    private void AddCardToHandUI(Card card)
    {
        // Assuming you have a method to instantiate card GameObjects properly
        GameObject cardGO = InstantiateCardGameObject(card);
        cardGO.transform.SetParent(handContainer.transform, false);
        // Add click listener for selection
        cardGO.GetComponent<Button>().onClick.AddListener(() => SelectCard(card));
    }

    private GameObject InstantiateCardGameObject(Card card)
    {
        GameObject cardGO = new GameObject("Card");
        var image = cardGO.AddComponent<Image>();
        var button = cardGO.AddComponent<Button>();
        image.sprite = GetCardSprite(card); // Your method to get the sprite
        return cardGO;
    }

    private void SelectCard(Card card)
    {
        selectedCard = card;
        // Highlight the selected card visually
        HighlightCard(card);
    }

    private void HighlightCard(Card card)
    {
        // Implementation to visually distinguish the selected card
    }
}
