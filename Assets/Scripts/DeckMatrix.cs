using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckMatrix : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject matrixUI; // Assign the parent GameObject of the matrix UI in the inspector

    // Serialized fields for the TextMeshProUGUI elements for ranks
    [Header("Ranks")]
    [SerializeField] private TextMeshProUGUI textAce;
    [SerializeField] private TextMeshProUGUI textTwo;
    [SerializeField] private TextMeshProUGUI textThree;
    [SerializeField] private TextMeshProUGUI textFour;
    [SerializeField] private TextMeshProUGUI textFive;
    [SerializeField] private TextMeshProUGUI textSix;
    [SerializeField] private TextMeshProUGUI textSeven;
    [SerializeField] private TextMeshProUGUI textEight;
    [SerializeField] private TextMeshProUGUI textNine;
    [SerializeField] private TextMeshProUGUI textTen;
    [SerializeField] private TextMeshProUGUI textJack;
    [SerializeField] private TextMeshProUGUI textQueen;
    [SerializeField] private TextMeshProUGUI textKing;


    // Serialized fields for the TextMeshProUGUI elements for suits
    [Header("Suits")]
    [SerializeField] private TextMeshProUGUI textHearts;
    [SerializeField] private TextMeshProUGUI textDiamonds;
    [SerializeField] private TextMeshProUGUI textClubs;
    [SerializeField] private TextMeshProUGUI textSpades;
    

    private void Start()
    {
        matrixUI.SetActive(false); // Hide on start
    }

    public void ToggleMatrix()
    {
        matrixUI.SetActive(!matrixUI.activeSelf);
        if (matrixUI.activeSelf)
        {
            UpdateMatrix();
        }
    }

    private void UpdateMatrix()
    {
        if (deckManager.playerDeck == null || deckManager.aiDeck == null)
        {
            Debug.LogError("Decks are not initialized.");
            return; // Prevent further execution if decks are null
        }
        var rankCounts = InitializeRankCounts();
        var suitCounts = InitializeSuitCounts();

        // Count the cards in the player's deck
        foreach (var card in deckManager.playerDeck)
        {
            rankCounts[card.rank]++;
            suitCounts[card.suit]++;
        }

        // Map each rank to its corresponding TextMeshProUGUI element
        var rankTexts = new Dictionary<string, TextMeshProUGUI>
        {
            {"Ace", textAce},
            {"2", textTwo},
            {"3", textThree},
            {"4", textFour},
            {"5", textFive},
            {"6", textSix},
            {"7", textSeven},
            {"8", textEight},
            {"9", textNine},
            {"10", textTen},
            {"Jack", textJack},
            {"Queen", textQueen},
            {"King", textKing}
        };

        // Update the matrix UI for ranks
        foreach (var rank in rankTexts.Keys)
        {
            rankTexts[rank].text = rankCounts[rank].ToString();
        }

        // Update the matrix UI for suits
        textHearts.text = suitCounts["Hearts"].ToString();
        textDiamonds.text = suitCounts["Diamonds"].ToString();
        textClubs.text = suitCounts["Clubs"].ToString();
        textSpades.text = suitCounts["Spades"].ToString();
        // ... update other suit texts
    }

    private Dictionary<string, int> InitializeRankCounts()
    {
        return new Dictionary<string, int>()
        {
            {"Ace", 0}, {"2", 0}, {"3", 0}, {"4", 0}, {"5", 0},
            {"6", 0}, {"7", 0}, {"8", 0}, {"9", 0}, {"10", 0},
            {"Jack", 0}, {"Queen", 0}, {"King", 0}
        };
    }

    private Dictionary<string, int> InitializeSuitCounts()
    {
        return new Dictionary<string, int>()
        {
            {"Hearts", 0}, {"Diamonds", 0}, {"Clubs", 0}, {"Spades", 0}
        };
    }
}