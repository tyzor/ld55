using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class DeckManager : HiddenSingleton<DeckManager>
{
    [SerializeField]
    private Deck playerDeck;
    [SerializeField]
    private Deck enemyDeck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void DrawPlayerCards(int num)
    {
        if (Instance.playerDeck == null) throw new System.Exception("No player deck assigned");
        Instance.playerDeck.DrawCards(num);
    }

    
}
