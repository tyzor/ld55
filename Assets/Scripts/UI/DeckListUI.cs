using System.Collections.Generic;
using Audio;
using Audio.SoundFX;
using UnityEngine;
using UnityEngine.UI;

public class DeckListUI : MonoBehaviour
{
    [SerializeField]
    private Deck deck;

    [SerializeField]
    private GridLayoutGroup layoutGroup;
    [SerializeField]
    private GameObject background;

    [SerializeField]
    private CardUI cardPrefab;

    [SerializeField]
    private Button toggleButton;

    void Start()
    {
        Hide();
        toggleButton.onClick.AddListener(OnButtonClick);
    }

    void OnEnable()
    {
        deck.DeckChangedAction += OnDeckCountChanged;
    }
    void OnDisable()
    {
        deck.DeckChangedAction -= OnDeckCountChanged;
    }

    private void ClearList()
    {
        foreach(Transform t in layoutGroup.transform)
        {
            Destroy(t.gameObject);
        }
    }

    private void RenderDeck(List<CardData> decklist)
    {
        ClearList();
        foreach(CardData card in decklist)
        {
            var c = Instantiate<CardUI>(cardPrefab, layoutGroup.transform);
            c.Render(card);
        }
    }

    void OnDeckCountChanged(List<CardData> decklist)
    {
        RenderDeck(decklist);
    }

    public void Show()
    {
        layoutGroup.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
    }

    public void Hide()
    {
        layoutGroup.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }

    public void Toggle()
    {
        layoutGroup.gameObject.SetActive(!layoutGroup.gameObject.activeSelf);
        background.gameObject.SetActive(!background.gameObject.activeSelf);
    }

    private void OnButtonClick()
    {
        Debug.Log("Deck button clicked");
        SFX.SELECTION.PlaySound();
        Toggle();
    }


}
