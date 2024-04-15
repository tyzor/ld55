using System.Collections;
using System.Collections.Generic;
using GameInput;
using TMPro;
using UnityEngine;

public enum CARDSUIT {
    None,
    Fire,
    Water,
    Earth,
    Metal,
    Wood
}

public class CardData
{

    // TODO -- change get to pull from modifier list
    public int value;
    
    public CARDSUIT suit = CARDSUIT.None;

    public CardData(int value, CARDSUIT suit)
    {
        this.value = value;
        this.suit = suit;
    }

    public static CardData RandomCard()
    {
        return new CardData(Random.Range(1,11), (CARDSUIT)Random.Range(1,5));
    }       

}

public class Card : MonoBehaviour
{
    public CardData cardData {
        get => _cardData;
        set {
            _cardData = value;
            RenderCard();
        }
    }
    private CardData _cardData;

    public int handPosition = -1;

    public bool IsAlive => _cardData.value > 0;

    [SerializeField]
    TextMeshProUGUI cardValueText;

    [SerializeField]
    CardVisualDataSO cardVisDataSO;

    private Material _frontMaterial;

    [SerializeField]
    private float hoverAnimationTime = .3f;
        [SerializeField]
    private float hoverScale = 1.2f;


    [SerializeField]
    LayerMask mouseHoverMask;

    private bool _hasHover;

    void Awake()
    {
        _frontMaterial = GetComponent<MeshRenderer>().materials[0];
    }

    private void RenderCard()
    {
        var visData = cardVisDataSO.GetData(cardData.suit);
        if(_frontMaterial != visData.frontTexture)
            _frontMaterial.SetTexture("_Texture", visData.frontTexture);
        if(cardValueText)
        {
            cardValueText.text = cardData.value.ToString(); 
            cardValueText.color = visData.fontColor;
        }
    }

    private IEnumerator HoverAnimation(float timer, bool over)
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = over ? Vector3.one * hoverScale : Vector3.one;
        for(float t=0;t<timer;t+=Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(startScale,targetScale,t/timer);

            // Early breaks if we have a change in value
            if(_hasHover != over) break;

            yield return null;
        }
    }

    public void SetInteractable(bool value) {
        GetComponent<Collider>().enabled = value;
    }

    void OnMouseOver()
    {
        //Debug.Log("MouseOver collider event");
        if(_hasHover) return;
        _hasHover = true;
        TriggerHover(true);   
    }
    void OnMouseExit()
    {
        //Debug.Log("MouseExit collider event");
        if(!_hasHover) return;
        _hasHover = false;
        TriggerHover(false);
    }

    void TriggerHover(bool enter)
    {
        var mask = 1 << gameObject.layer;
        if( (mask & mouseHoverMask.value) != 0)
        {
            StartCoroutine(HoverAnimation(hoverAnimationTime,enter));
        }
    }

    public void TakeDamage(int damage)
    {
        cardData.value = Mathf.Max(0,cardData.value-damage);
        RenderCard();
    }

}
