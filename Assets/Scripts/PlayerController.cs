using System;
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
    
    [SerializeField]
    protected AnimationCurve cardCaptureCurve;

    [SerializeField]
    protected float cardAttackAnimationTime = 0.5f;

    public Card playedCard = null;

    [SerializeField]
    LayerMask cardLayerMask;

    public int cardDrawStrength = 1;

    public event Action<int> EnergyGainedAction; 


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
        cardDrawStrength = isPlayer ? 2 : 1;
    }


    // Draw cards and put them to hand -- animate
    public Coroutine DrawCards()
    {
        return StartCoroutine(DrawCardsCoroutine());
    }

    public IEnumerator DrawCardsCoroutine()
    {
        Debug.Log($"{name} drawing {cardDrawStrength} cards");
        var cards = deck.DrawCards(cardDrawStrength);
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
        // TODO -- clean this up in separate class
        // we need to set the values to the final position (in cases we skipped the last partial move)
        for(int i=0;i<cards.Count;i++){
            cards[i].transform.position = handPositions[i];
            cards[i].transform.rotation = Quaternion.identity;
        }

        // Make the cards interactable
        for(int i=0;i<cards.Count;i++)
        {
            cards[i].SetInteractable(true);
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
        // Make hand lock from interaction
        for(int i=0;i<_hand.cards.Count;i++)
        {
            _hand.cards[i].SetInteractable(false);
        }

        Quaternion targetRot = Quaternion.Euler(new Vector3(0,0,180));
        var handPositions = _hand.GetHandPositions(_hand.cards.Count);
        for(float t=0;t<cardPlayAnimationTime;t+=Time.deltaTime)    
        {
            for(int i=0;i<_hand.cards.Count;i++)
            {
                if(_hand.cards[i] == card)
                {
                    _hand.cards[i].transform.position = Vector3.Lerp(handPositions[i], _hand.transform.position + Vector3.up * 0.5f, t/cardPlayAnimationTime);
                    continue;
                };
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
        card.transform.position = playedCardPosition.position;

        // Put cards back in deck and clear hand
        _hand.cards.Remove(card);
        card.handPosition = -1;
        deck.AddCardsToBottom(_hand.cards.ToArray());
        foreach(Card c in _hand.cards)
        {
            Destroy(c.gameObject);
        }
        _hand.cards.Clear();

    }

    public Coroutine BattleCards(PlayerController other)
    {
        return StartCoroutine(BattleCardsCoroutine(other));
    }

    private IEnumerator BattleCardsCoroutine(PlayerController other)
    {
        var targetCard = other.playedCard;
        var otherDeck = other.deck;

        // Do animation for capturing
        // For now we only capture one card -- TODO maybe more?
        var card1Start = playedCard.transform.position;
        var card2Start = targetCard.transform.position;

        var midPoint = (playedCard.transform.position + targetCard.transform.position)/2f;
        
        var card1Diff = midPoint - card1Start;
        var card2Diff = midPoint - card2Start;
        var c1Dir = card1Diff.normalized;
        var c2Dir = card1Diff.normalized;

        for(float t=0;t<cardAttackAnimationTime;t+=Time.deltaTime)
        {
            playedCard.transform.position = card1Start + card1Diff * cardCaptureCurve.Evaluate(t/cardAttackAnimationTime);
            targetCard.transform.position = card2Start + card2Diff * cardCaptureCurve.Evaluate(t/cardAttackAnimationTime);
            /*
            playedCard.transform.position = new Vector3(
                    playedCard.transform.position.x, 
                    Mathf.Lerp(startPosition.y, targetCard.transform.position.y + 0.05f, t/cardAttackAnimationTime), 
                    startPosition.z + zDiff * this.cardCaptureCurve.Evaluate(t/cardAttackAnimationTime)
                );
                */
            yield return null;
        }

        // Cards deal damage to each other
        var c1Value = playedCard.cardData.value;
        var c2Value = targetCard.cardData.value;
        playedCard.cardData.value -= Mathf.Min(c2Value,0);
        targetCard.cardData.value -= Mathf.Min(c1Value,0);

        yield return new WaitForSeconds(.2f);
        
        // Send cards back to deck
        for(float t=0;t<cardPlayAnimationTime;t+=Time.deltaTime)    
        {
            if(playedCard.cardData.value > 0)
                playedCard.transform.position = Vector3.Lerp(card1Start, deck.transform.position, t/cardPlayAnimationTime);
            if(targetCard.cardData.value > 0)
                targetCard.transform.position = Vector3.Lerp(card2Start, otherDeck.transform.position, t/cardPlayAnimationTime);
            yield return null;
        }

        // Add cards to bottom of deck
        if(playedCard.cardData.value > 0)
            deck.AddCardToBottom(playedCard);
        if(targetCard.cardData.value > 0)
            otherDeck.AddCardToBottom(targetCard);
        
        Destroy(playedCard.gameObject);
        Destroy(targetCard.gameObject);

        yield return null;
    }

    public Coroutine BattleTied() 
    {
        return StartCoroutine(BattleTiedCoroutine());
    }

    private IEnumerator BattleTiedCoroutine() 
    {
        // Send played card back to deck
        for(float t=0;t<cardPlayAnimationTime;t+=Time.deltaTime)    
        {
            playedCard.transform.position = Vector3.Lerp(playedCardPosition.position, deck.transform.position, t/cardPlayAnimationTime);
            yield return null;
        }

        Card[] cards = { playedCard };
        deck.AddCardsToBottom(cards);

        Destroy(playedCard.gameObject);
    }

    public bool CanDraw()
    {
        return deck.CanDraw(cardDrawStrength);
    }

}
