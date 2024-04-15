using System.Collections;
using System.Collections.Generic;
using Audio;
using Audio.SoundFX;
using UnityEngine;

public class EnemyController : PlayerController
{
    public bool hidePlayedCard = false;


    // Basic strategy for enemies -- draw and play
    public Coroutine PlayTopCard()
    {
        return StartCoroutine(PlayTopCardCoroutine());
    }


    private IEnumerator PlayTopCardCoroutine()
    {
        var cards = deck.DrawCards(1);

        var startPosition = deck.GetTopCardPosition();
        var startRotation = Quaternion.Euler(new Vector3(0,0,180));
        yield return AnimationUtils.Lerp(cardDrawAnimationTime, (t) => {
            // Adding parabolic up offset to make sure cards don't clip the ground
            cards[0].transform.position = Vector3.Lerp(startPosition, playedCardPosition.position, t) + (Vector3.up * cardFlipHeightScale * cardFlipHeightCurve.Evaluate(t) );
            cards[0].transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, hidePlayedCard ? 0 : t );
        });
        playedCard = cards[0];
        SFX.CARD_PLACE.PlaySound();
    }

    public Coroutine FlipPlayedCard()
    {
        return StartCoroutine(FlipPlayedCardCoroutine());
    }


    private IEnumerator FlipPlayedCardCoroutine()
    {
        if(!hidePlayedCard) yield break;

        SFX.CARD_FLIP.PlaySound();
        var startPosition = playedCard.transform.position;
        var startRotation = playedCard.transform.rotation;
        yield return AnimationUtils.Lerp(cardDrawAnimationTime, (t) => {
            // Adding parabolic up offset to make sure cards don't clip the ground
            playedCard.transform.position =  startPosition + (Vector3.up * cardFlipHeightScale * cardFlipHeightCurve.Evaluate(t) );
            playedCard.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);
        });
    }
    
    protected override IEnumerator SummonCoroutine(PlayerController otherPlayer)
    {
        if(!CanSummon()) yield break;

        
        SummonData summon = summonUI.MakeAIChoice();
        summonListUI.AddSummon(summon.type);
        

        yield return StartCoroutine(ApplySummonEffect(summon.type, otherPlayer));

    }


}
