using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<CardData> decklist;
    [SerializeField]
    private Card cardPrefab;
    

    public void Init()
    {
         
        Debug.Log($"{name} deck is initializing ");
        decklist = new List<CardData>();    
        
        for(int i=0;i<10;i++)
        {
            decklist.Add(CardData.RandomCard());
        }
    }

    public List<Card> DrawCards(int num) {

        // TODO -- trigger event if we have no draws left

        List<Card> draws = new List<Card>();
        Debug.Log($"Drawing {num} cards from {name}");

        for(var i=0; i<num;i++)
        {
            var cardData = decklist[0];
            decklist.RemoveAt(0);
            Card card = Instantiate<Card>(cardPrefab,transform.parent);
            card.transform.position = GetTopCardPosition();
            card.cardData = cardData;
            draws.Add(card);
        }

        return draws;

    }

    public Vector3 GetTopCardPosition() {

        Vector3 result = Vector3.zero;

        foreach(Transform child in transform)
        {
            if(child.position.y > result.y)
            {
                result = child.position;
            }
        }

        // Offset slightly to avoid z fighting
        return result + Vector3.up * 0.01f;
    }

    private void CreateTestCard() {

        


    }
}