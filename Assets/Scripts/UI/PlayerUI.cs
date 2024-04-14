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
        player.EnergyGainedAction += OnEnergyChanged;
    }
    void OnDisable()
    {
        deck.DeckChangedAction -= OnDeckCountChanged;
        player.EnergyGainedAction -= OnEnergyChanged;
    }


    private void OnDeckCountChanged(List<CardData> deckList)
    {
        deckCountLabel.text = $"{deckList.Count}";
    }

    private void OnEnergyChanged(int count)
    {
        energyCountLabel.text = $"{Mathf.Max(count,10)/10}";
    }
}
