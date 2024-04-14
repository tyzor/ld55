using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlayerController
{

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
        for(float t=0;t<cardDrawAnimationTime;t+=Time.deltaTime)
        {
            // Adding parabolic up offset to make sure cards don't clip the ground
            cards[0].transform.position = Vector3.Lerp(startPosition, playedCardPosition.position, t / cardDrawAnimationTime) + (Vector3.up * cardFlipHeightScale * cardFlipHeightCurve.Evaluate(t/cardDrawAnimationTime) );
            cards[0].transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t/cardDrawAnimationTime);
            yield return null;
        }
        cards[0].transform.position = playedCardPosition.position;
        cards[0].transform.rotation = Quaternion.identity;

        playedCard = cards[0];

        Debug.Log("playtopcardcoroutine finished");
    }

}
