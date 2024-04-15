using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SummonUI : ChoiceUI
{
    [SerializeField]
    private SummonController summonController;

    private SummonData[] _summons;

    [SerializeField]
    private TextMeshProUGUI[] choicesText;
    [SerializeField]
    private Image[] choicesImage;

    public void Setup()
    {
        _summons = summonController.GetRandomSummonChoices();
        
        for(int i=0;i<choices.Length;i++)
        {
            choicesText[i].text = "-"+ _summons[i].name.ToUpper()+"-<br>"+_summons[i].description;
            choicesImage[i].sprite = _summons[i].image;
        }
    }

    public SummonData GetChoice() => _summons[selectedIndex];
    
    // TODO -- reduce AI choices based on difficulty?
    public SummonData MakeAIChoice() {
        SummonData[] aiChoices = summonController.GetRandomSummonChoices();
        return aiChoices[0];
    }
}
