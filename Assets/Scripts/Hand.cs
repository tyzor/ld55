using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Bounds _bounds;
    private Bounds _cardBounds;

    [SerializeField]
    private float maxDistance = .5f;

    [SerializeField]
    private Card cardPrefab;

    public List<Card> cards = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        _bounds = GetComponent<BoxCollider>().bounds;
        _cardBounds = cardPrefab.GetComponent<MeshRenderer>().bounds;
    }

    // Return positions for each card distributed across the bounds
    public Vector3[] GetHandPositions(int numCards)
    {
        List<Vector3> handPos = new List<Vector3>();

        float handSize = _bounds.extents.x * 2f;
        float cardSize = _cardBounds.extents.x * 2f;
        float spacing = Mathf.Min((handSize - (cardSize * numCards)) / (numCards - 1), maxDistance);
        float totalHandSize = cardSize * numCards + spacing * (numCards-1);
        float leftOffset = (handSize - totalHandSize) / 2f - handSize / 2f;

        Debug.Log($"{handSize} - {totalHandSize} - {spacing} - {cardSize} - {leftOffset}");

        for(int i=0; i<numCards;i++)
        {
            handPos.Add(new Vector3(leftOffset + cardSize*i + spacing*i, _bounds.center.y, _bounds.center.z));
        }

        return handPos.ToArray();
        
    }

}
