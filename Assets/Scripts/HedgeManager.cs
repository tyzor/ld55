using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class HedgeManager : MonoBehaviour
{
    public TextMeshProUGUI[] effectDisplays = new TextMeshProUGUI[3];
    private List<string> effectsList = new List<string> { "Effect 1", "Effect 2", "Effect 3" };

    public void SeeEffects()
    {
        for (int i = 0; i < effectDisplays.Length; i++)
        {
            if (i < effectsList.Count)
                effectDisplays[i].text = effectsList[i];
        }
    }
}
