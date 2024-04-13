using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardHolder;
using System.Globalization;
using TMPro;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject[] cardHolders; // Array of GameObjects that represent card slots in the UI
    public List<Card> playerHand = new List<Card>();
    public GameObject selectedCardObject; // GameObject of the currently selected card

    public void DrawCardsToHand(int count)
    {
        if (deckManager == null || deckManager.playerDeck == null)
        {
            Debug.LogError("DeckManager or playerDeck is not properly initialized.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            if (deckManager.playerDeck.Count > 0 && i < cardHolders.Length && cardHolders[i] != null)
            {
                Card card = deckManager.DrawCard();
                if (card != null)
                {
                    playerHand.Add(card);
                    GameObject cardObject = DisplayCardInUI(card, i);
                    cardHolders[i].SetActive(true); // Ensure the card slot is active
                }
                else
                {
                    Debug.LogError("Drawn card is null.");
                }
            }
            else
            {
                if (i >= cardHolders.Length || cardHolders[i] == null)
                    Debug.LogError("Card holder is not set or out of index: " + i);
                if (deckManager.playerDeck.Count <= 0)
                    Debug.LogError("No more cards to draw.");
            }
        }
    }


    private GameObject DisplayCardInUI(Card card, int index)
    {
        GameObject cardObject = cardHolders[index];
        RawImage imageComponent = cardObject.GetComponent<RawImage>();
        imageComponent.texture = deckManager.GetCardTexture(card);
        cardObject.SetActive(true); // Ensure the card is visible
        return cardObject;
    }

    private void SelectCard(Card card, GameObject cardObject)
    {
        if (selectedCardObject != null)
        {
            // Disable the highlight on previously selected card
            selectedCardObject.transform.Find("Highlight").gameObject.SetActive(false);
        }

        selectedCardObject = cardObject;
        // Enable the highlight on the new selected card
        selectedCardObject.transform.Find("Highlight").gameObject.SetActive(true);
    }

    public void HighlightCard(Card card)
    {
        // Find the card object in the hand
        int index = playerHand.IndexOf(card);
        if (index != -1)
        {
            SelectCard(card, cardHolders[index]);
        }
    }

    public void ClearHandDisplay()
    {
        foreach (GameObject holder in cardHolders)
        {
            holder.SetActive(false); // Hide all card holders
            holder.transform.Find("Highlight").gameObject.SetActive(false); // Ensure highlights are turned off
        }
        playerHand.Clear();
    }
}
