using System;
using System.Collections;
using System.Collections.Generic;
using GameInput;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected Deck deck;

    [SerializeField]
    private Hand _hand;
    
    private int _energy;
    private int _summonCost;
    private int _energyMultiplierWin = 1;
    private int _energyMultiplierLose = 1;

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

    public event Action<int, int> EnergyChangedAction; 

    public bool CanDraw() => deck.CanDraw(cardDrawStrength);
    public bool CanSummon() => _energy >= _summonCost;

    [SerializeField]
    protected SummonUI summonUI;

    private List<SUMMON_TYPE> _summonsList;

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
        summonUI.Hide();
        deck.Init();
        cardDrawStrength = isPlayer ? 2 : 1;
        _energy = 0;
        _summonCost = 10;
        _energyMultiplierWin = 1;
        _energyMultiplierLose = 1;
        _summonsList = new List<SUMMON_TYPE>();
        
        EnergyChangedAction?.Invoke(_energy, _summonCost);
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
        yield return AnimationUtils.Lerp(cardDrawAnimationTime, t => {
            for(int i=0;i<cards.Count;i++)
            {
                cards[i].transform.position = Vector3.Lerp(startPosition, handPositions[i], t) + (Vector3.up * cardFlipHeightScale * cardFlipHeightCurve.Evaluate(t));
                cards[i].transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);
                cards[i].handPosition = i;
            }
        });

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

        yield return AnimationUtils.Lerp(cardPlayAnimationTime, t => {
            for(int i=0;i<_hand.cards.Count;i++)
            {
                if(_hand.cards[i] == card)
                {
                    _hand.cards[i].transform.position = Vector3.Lerp(handPositions[i], _hand.transform.position + Vector3.up * 0.5f, t);
                    continue;
                };
                _hand.cards[i].transform.position = Vector3.Lerp(handPositions[i], deck.transform.position, t);
                _hand.cards[i].transform.rotation = Quaternion.Lerp(Quaternion.identity, targetRot, t);
            }
        });

        var startPosition = card.transform.position;
        yield return AnimationUtils.Lerp(cardPlayAnimationTime, t => {
            card.transform.position = Vector3.Lerp(startPosition, playedCardPosition.position, t);
        });

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

    private IEnumerator BattleCardsCoroutine(PlayerController otherPlayer)
    {
        var targetCard = otherPlayer.playedCard;
        var otherDeck = otherPlayer.deck;

        // Do animation for capturing
        // For now we only capture one card -- TODO maybe more?
        var card1Start = playedCard.transform.position;
        var card2Start = targetCard.transform.position;

        var midPoint = (playedCard.transform.position + targetCard.transform.position)/2f;
        
        var card1Diff = midPoint - card1Start;
        var card2Diff = midPoint - card2Start;
        var c1Dir = card1Diff.normalized;
        var c2Dir = card1Diff.normalized;

        yield return AnimationUtils.Lerp(cardAttackAnimationTime, t => {
            playedCard.transform.position = card1Start + card1Diff * t;
            targetCard.transform.position = card2Start + card2Diff * t;
        }, curve: cardCaptureCurve);

        // Cards deal damage to each other
        var c1Value = playedCard.cardData.value;
        var c2Value = targetCard.cardData.value;
        playedCard.TakeDamage(c2Value);
        targetCard.TakeDamage(c1Value);

        // Apply energy
        GainEnergy(playedCard.IsAlive ? playedCard.cardData.value : 1, playedCard.IsAlive);
        otherPlayer.GainEnergy(targetCard.IsAlive ? targetCard.cardData.value : 1, targetCard.IsAlive);
        
        yield return new WaitForSeconds(.2f);
        
        // Send cards back to deck
        yield return AnimationUtils.Lerp(cardPlayAnimationTime, t => {
            if(playedCard.IsAlive)
                playedCard.transform.position = Vector3.Lerp(card1Start, deck.transform.position, t);
            if(targetCard.IsAlive)
                targetCard.transform.position = Vector3.Lerp(card2Start, otherDeck.transform.position, t);
        });

        // Add cards to bottom of deck
        if(playedCard.cardData.value > 0)
            deck.AddCardToBottom(playedCard);
        if(targetCard.cardData.value > 0)
            otherDeck.AddCardToBottom(targetCard);
        
        Destroy(playedCard.gameObject);
        Destroy(targetCard.gameObject);

        yield return null;
    }

    protected void GainEnergy(int value, bool isWin)
    {
        int multiplier = isWin ? _energyMultiplierWin : _energyMultiplierLose;
        _energy = Mathf.Clamp(_energy + value * multiplier, 0, _summonCost);
        EnergyChangedAction?.Invoke(_energy, _summonCost);
    }

    public Coroutine TrySummon(PlayerController otherPlayer) => StartCoroutine(SummonCoroutine(otherPlayer));
    protected virtual IEnumerator SummonCoroutine(PlayerController otherPlayer)
    {
        if(!CanSummon()) yield break;

        // Present summon interface
        // TODO -- fade in
        summonUI.Show();
        summonUI.Setup();

        while(summonUI.selectedIndex < 0)
        {
            yield return null;
        }

        SummonData summon = summonUI.GetChoice();
        summonUI.selectedIndex = -1;
        summonUI.Hide(); // hide ui
        _summonsList.Add(summon.type);
        yield return StartCoroutine(ApplySummonEffect(summon.type, otherPlayer));

    }

    protected IEnumerator ApplySummonEffect(SUMMON_TYPE summonType, PlayerController enemy)
    {
        switch(summonType)
        {
            case SUMMON_TYPE.DOG:
                _summonCost = 5;
                _energy = Mathf.Clamp(_energy,0,_summonCost);
                EnergyChangedAction?.Invoke(_energy, _summonCost);
                break;
            case SUMMON_TYPE.DRAGON:
                // TODO -- Display choice selection
                break;
            case SUMMON_TYPE.HORSE:
                cardDrawStrength = Mathf.Clamp(cardDrawStrength + 1,0,5);
                break;
            case SUMMON_TYPE.MONKEY:
                deck.RandomizeValues();
                //TODO -- shuffle enemy deck?
                break;
            case SUMMON_TYPE.OX:
                //TODO -- element selection
                break;
            case SUMMON_TYPE.PIG:
                deck.RaiseTopCard(8);
                break;
            case SUMMON_TYPE.RABBIT:
                deck.RaisePower(3,1);
                break;
            case SUMMON_TYPE.RAT:
                deck.SplitDeck();
                enemy.deck.SplitDeck();
                break;
            case SUMMON_TYPE.ROOSTER:
                _energyMultiplierWin++;
                break;
            case SUMMON_TYPE.SHEEP:
                deck.RaiseRandom(4,3);
                break;
            case SUMMON_TYPE.SNAKE:
                _energyMultiplierLose++;
                break;
            case SUMMON_TYPE.TIGER:
                deck.TransmuteCards(5);
                break;
            
        }

        // Subtract the cost
        _energy = 0;
        if(_summonCost != 10 && summonType != SUMMON_TYPE.DOG)
        {
            _summonCost = 10;
        }
        EnergyChangedAction?.Invoke(_energy, _summonCost);

        yield return null;
    }

}
