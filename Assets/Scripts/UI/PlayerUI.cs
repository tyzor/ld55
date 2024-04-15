using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI deckCountLabel;
    [SerializeField]
    private TextMeshProUGUI energyCountLabel;

    [SerializeField]
    private Deck deck;

    [SerializeField]
    private PlayerController player;

    void OnEnable()
    {
        deck.DeckChangedAction += OnDeckCountChanged;
        player.EnergyChangedAction += OnEnergyChanged;
    }
    void OnDisable()
    {
        deck.DeckChangedAction -= OnDeckCountChanged;
        player.EnergyChangedAction -= OnEnergyChanged;
    }


    private void OnDeckCountChanged(List<CardData> deckList)
    {
        deckCountLabel.text = $"{deckList.Count}";
    }

    private void OnEnergyChanged(int count, int cost)
    {
        energyCountLabel.text = $"{Mathf.Min(count,cost)}/{cost}";
    }
}
