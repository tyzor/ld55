using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necessary for working with UI elements like RawImage and Button
using CardHolder;

public class HandManager : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager
    public GameObject[] cardHolders; // Array of GameObjects that will hold the cards on the UI
    public RawImage selectionHighlight; // The RawImage that will be used as a highlight for selected card

    private int selectedIndex = -1; // Index of the selected card

    // Method called to draw multiple cards to the hand
    public void DrawCardsToHand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Card card = gameManager.deckManager.DrawCard(); // Assume DrawCard returns a Card object
            // Display the card on the UI, using a method that sets the image on the RawImage
            UpdateCardDisplay(cardHolders[i], card);

            // Set up the button on-click event
            int index = i; // Local copy for correct closure capture
            cardHolders[i].GetComponent<Button>().onClick.AddListener(() => SelectCard(index));
        }
    }

    // This method will handle the logic when a card is selected from the hand
    private void SelectCard(int index)
    {
        // Deselect the previously selected card
        if (selectedIndex != -1)
        {
            DeselectCard(selectedIndex);
        }

        // Select the new card
        selectedIndex = index;
        selectionHighlight.gameObject.SetActive(true); // Turn on the highlight
        selectionHighlight.transform.position = cardHolders[selectedIndex].transform.position; // Move highlight to selected card

        // Notify GameManager of the selected card
        gameManager.SelectCardForBattle(gameManager.deckManager.playerDeck[selectedIndex]);
    }

    // Method called to update the UI card display
    private void UpdateCardDisplay(GameObject cardHolder, Card card)
    {
        var cardImage = cardHolder.GetComponent<RawImage>();
        cardImage.texture = gameManager.deckManager.GetCardTexture(card);
        cardHolder.SetActive(true); // Make the card visible
    }

    // Method to deselect a card, hiding its highlight
    private void DeselectCard(int index)
    {
        // Assuming we're just hiding the selection highlight here
        if (index == selectedIndex)
        {
            selectionHighlight.gameObject.SetActive(false);
        }
    }

    // Clear the hand display, including deselecting any selected card
    public void ClearHandDisplay()
    {
        foreach (var holder in cardHolders)
        {
            holder.SetActive(false); // Deactivate the card holder
            holder.GetComponent<Button>().onClick.RemoveAllListeners(); // Remove all listeners to avoid memory leaks
        }
        if (selectedIndex != -1)
        {
            DeselectCard(selectedIndex);
        }
        selectedIndex = -1; // Reset the selected index
    }

    // Add other necessary methods...
}