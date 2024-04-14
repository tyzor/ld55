using System.Collections;
using System.Collections.Generic;
using GameInput;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected Deck deck;

    [SerializeField]
    private Hand _hand;
    
    [SerializeField]
    protected Transform playedCardPosition;

    public bool isPlayer = false;

    [SerializeField]
    protected float cardDrawAnimationTime = 1f;
    [SerializeField]
    protected AnimationCurve cardFlipHeightCurve;
    [SerializeField]
    protected float cardFlipHeightScale = 1f;

    [SerializeField]
    protected float cardPlayAnimationTime = 1f;

    public Card playedCard = null;

    [SerializeField]
    LayerMask cardLayerMask;

    private void OnEnable()
    {
        GameInputDelegator.OnLeftClick += OnLeftClick;
    }
    
    private void OnDisable()
    {
        GameInputDelegator.OnLeftClick -= OnLeftClick;
    }

    public void Init()
    {
        // Do any setup here
        deck.Init();
    }


    // Draw cards and put them to hand -- animate
    public Coroutine DrawCards(int num)
    {
        return StartCoroutine(DrawCardsCoroutine(num));
    }

    public IEnumerator DrawCardsCoroutine(int num)
    {
        Debug.Log($"{name} drawing {num} cards");
        var cards = deck.DrawCards(num);
        _hand.cards = cards;

        foreach(Card card in _hand.cards)
        {
            card.gameObject.layer = LayerMask.NameToLayer("Card");
        }

        var startPosition = deck.GetTopCardPosition();
        var startRotation = Quaternion.Euler(new Vector3(0,0,-180));
        var handPositions =  _hand.GetHandPositions(cards.Count);
        for(float t=0;t<cardDrawAnimationTime;t+=Time.deltaTime)
        {
            for(int i=0;i<cards.Count;i++)
            {
                cards[i].transform.position = Vector3.Lerp(startPosition, handPositions[i], t / cardDrawAnimationTime) + (Vector3.up * cardFlipHeightScale * cardFlipHeightCurve.Evaluate(t/cardDrawAnimationTime));
                cards[i].transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t/cardDrawAnimationTime);
                cards[i].handPosition = i;
            }
           yield return null;
        }
    }

    private void OnLeftClick(bool pressed)
    {
        // pressed false is mouseup
        if(playedCard == null && !pressed)
        {
            // Cast a ray and see if we hit any cards
            // First we determine where the mouse cursor is located
            var screenPointToRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(!Physics.Raycast(screenPointToRay, out RaycastHit raycastHit, 100f, cardLayerMask ) )
            {
                return;
            }

            if(!raycastHit.transform.TryGetComponent<Card>(out Card card)) return;
        
            playedCard = card;

        }
    }

    public Coroutine PlayCard(Card card)
    {
        return StartCoroutine(PlayCardCoroutine(card));
    }

    private IEnumerator PlayCardCoroutine(Card card)
    {

        // Send other cards back to deck
        Quaternion targetRot = Quaternion.Euler(new Vector3(0,0,180));
        var handPositions = _hand.GetHandPositions(_hand.cards.Count);
        for(float t=0;t<cardPlayAnimationTime;t+=Time.deltaTime)    
        {
            for(int i=0;i<_hand.cards.Count;i++)
            {
                if(_hand.cards[i] == card) continue;
                _hand.cards[i].transform.position = Vector3.Lerp(handPositions[i], deck.transform.position, t / cardPlayAnimationTime);
                _hand.cards[i].transform.rotation = Quaternion.Lerp(Quaternion.identity, targetRot, t/cardPlayAnimationTime);
            }
            yield return null;
        }

        var startPosition = card.transform.position;
        for(float t=0;t<cardPlayAnimationTime;t+=Time.deltaTime)
        {
            card.transform.position = Vector3.Lerp(startPosition, playedCardPosition.position, t / cardPlayAnimationTime);
            yield return null;
        }

    }

}
