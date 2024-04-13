using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necessary for working with UI elements like RawImage and Button
using CardHolder;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameManager gameManager; // Reference to the GameManager
    public GameObject[] cardHolders; // Array of GameObjects that will hold the cards on the UI
    public RawImage selectionHighlight; // The RawImage that will be used as a highlight for selected card
    public RawImage largeCardDisplay; // The large central display for the selected card
    public Button[] cardButtons; // Assign buttons for each card in the Inspector


    private int selectedIndex = -1; // Index of the selected card

    private void Start()
    {
        // Initialize by hiding the selection highlight
        selectionHighlight.gameObject.SetActive(false);
    }

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

    public void OnCardClicked(int cardIndex)
    {
        // Check if the cardIndex is valid
        if (cardIndex < 0 || cardIndex >= cardButtons.Length)
        {
            Debug.LogError("Card index is out of bounds.");
            return;
        }

        // Highlight the selected card
        HighlightCard(cardIndex);

        Card selectedCard = gameManager.deckManager.playerDeck[cardIndex]; // Fetch the card data
        gameManager.UpdateLargeCardDisplay(selectedCard);
    }

    private void ResetHighlights()
    {
        foreach (var cardHolder in cardHolders)
        {
            // This assumes each cardHolder has a child GameObject named 'Highlight' which has a RawImage
            var highlight = cardHolder.transform.Find("Highlight").GetComponent<RawImage>();
            if (highlight != null)
            {
                highlight.enabled = false;
            }
        }
    }

    // Update the display of the large card
    private void UpdateLargeCardDisplay(Card card)
    {
        Texture2D cardImage = deckManager.GetCardTexture(card); // Get the card image
        largeCardDisplay.texture = cardImage; // Update the large card display
    }

    // Highlight the selected card
    private void HighlightCard(int cardIndex)
    {
        if (selectedIndex != -1)
        {
            // Disable the highlight for the previously selected card
            ToggleHighlight(selectedIndex, false);
        }

        // Enable the highlight for the new selected card
        ToggleHighlight(cardIndex, true);
        selectedIndex = cardIndex; // Update the selected index
    }

    // Toggle the highlight state for a card
    private void ToggleHighlight(int cardIndex, bool state)
    {
        // Assume the highlight is a child of the button, hence we use 'transform.GetChild(0)'
        // If it's elsewhere in the hierarchy, you'll need to adjust this
        Transform highlightTransform = cardButtons[cardIndex].transform.GetChild(0);
        if (highlightTransform != null)
        {
            RawImage highlightImage = highlightTransform.GetComponent<RawImage>();
            if (highlightImage != null)
            {
                highlightImage.gameObject.SetActive(state); // Show/hide the highlight
            }
        }
    }
}
