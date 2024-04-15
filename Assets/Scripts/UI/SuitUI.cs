using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SuitUI : ChoiceUI
{
    [SerializeField]
    private CardVisualDataSO cardVisDataSO;

    public CARDSUIT GetSelection() => cardVisDataSO.cardTextures[selectedIndex].suit;

    // Start is called before the first frame update
    void Awake()
    {
        // Setup the choices here
        for(int i=0;i<choices.Length;i++)
        {
            Button btn = choices[i];
            RawImage img = btn.GetComponentInChildren<RawImage>();
            img.texture = cardVisDataSO.cardTextures[i].frontTexture;
        }    
    }

    
}
