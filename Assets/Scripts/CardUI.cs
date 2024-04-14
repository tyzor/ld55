using UnityEngine;
using UnityEngine.UI;
using CardHolder;

public class CardUI : MonoBehaviour
{
    public Text cardText; // Assuming there's a Text component to display card info

    // A method to setup the card's UI
    public void SetupCardUI(Card card, int index, HandManager handManager)
    {
        if (card != null && cardText != null)
        {
            cardText.text = card.rank + " of " + card.suit; // Simple text representation
        }
        else
        {
            Debug.LogError("Card or cardText is null!");
        }
    }
}
