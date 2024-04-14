using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CARDSUIT {
    None,
    Fire,
    Water,
    Earth,
    Wind
}

public class CardData
{

    // TODO -- change get to pull from modifier list
    public int value;
    
    public CARDSUIT suit = CARDSUIT.None;

    public CardData(int value, CARDSUIT suit)
    {
        this.value = value;
        this.suit = suit;
    }

    public static CardData RandomCard()
    {
        return new CardData(Random.Range(1,11), (CARDSUIT)Random.Range(1,5));
    }       

}

public class Card : MonoBehaviour
{
    public CardData cardData {
        get => _cardData;
        set {
            _cardData = value;
            if(cardValueText)
                cardValueText.text = value.value.ToString(); 
        }
    }
    private CardData _cardData;

    public int handPosition = -1;

    [SerializeField]
    TextMeshProUGUI cardValueText;

    void Start()
    {

    }

}
