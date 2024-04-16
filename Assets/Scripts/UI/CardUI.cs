using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private RawImage background;

    [SerializeField]
    private CardVisualDataSO cardSO;

    public void Render(CardData data)
    {
        text.text = data.value.ToString();
        background.texture = cardSO.GetData(data.suit).frontTexture;
    }
}
