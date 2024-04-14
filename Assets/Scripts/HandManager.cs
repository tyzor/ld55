using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necessary for working with UI elements like RawImage and Button
using CardHolder;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameManager gameManager; // Reference to the GameManager
    public GameObject[] cardHolders; // Array of GameObjects that will hold the cards on the UI
    public Button[] cardButtons; // Assign buttons for each card in the Inspector
    public GameObject[] cardObjectsInHand = new GameObject[3];
    private int selectedIndex = -1; // Index of the selected card

        // Assuming you have a class Card that stores the card's details
    [SerializeField] private GameObject cardPrefab; // Your card prefab with UI components
    [SerializeField] private Transform handTransform; // Parent transform to hold the card prefabs in the hand UI
    [SerializeField] private RawImage largeCardDisplay; // UI component to display the selected card in large view
    [SerializeField] private RawImage selectionHighlight; // Highlight image that can be enabled/disabled

    private List<Card> cardsInHand = new List<Card>();
    private List<GameObject> cardGameObjects = new List<GameObject>();

    void Start()
    {

    }

public void DrawCardsToHand(int numberOfCards)
{
    for (int i = 0; i < numberOfCards; i++)
    {
        // Using the correct method from DeckManager to draw a card
        Card card = deckManager.DrawCard(); // Changed from DrawCardFromDeck
        cardsInHand.Add(card);

        // Instantiate the card UI prefab and set up its visuals
        GameObject cardGO = Instantiate(cardPrefab, handTransform);
        cardGameObjects.Add(cardGO);
        
        // Assume that your card prefab has a method 'SetupCardUI' that takes a Card object
        if (cardGO.GetComponent<CardUI>() != null)
        {
            cardGO.GetComponent<CardUI>().SetupCardUI(card, i, this);
        }
        else
        {
            Debug.LogError("CardUI component missing on card prefab!");
        }
    }
}


    // Call this method from the card UI prefab when clicked, passing the index of the card
    public void SelectCard(int index)
    {
        if (index < 0 || index >= cardsInHand.Count)
        {
            Debug.LogError($"SelectCard: No card at index {index}.");
            return;
        }

        // Reset all highlights before highlighting the selected card
        foreach (var cardGO in cardGameObjects)
        {
            //cardGO.GetComponent<CardUI>().SetHighlight(false);
        }

        Card selectedCard = cardsInHand[index];
        //cardGameObjects[index].GetComponent<CardUI>().SetHighlight(true);

        // Update the large card display with the selected card
        UpdateLargeCardDisplay(selectedCard);
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
