using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    // list of all Card objects
    private List<Card> cards = new List<Card>();
    
    public void Initialize()
    {
        // start games with the cards cleared from cache
        cards.Clear();
        
        // make an array of CardColor type (minus wild cards)
        CardColor[] standardColors = new CardColor[4];
        standardColors[0] = CardColor.Red;
        standardColors[1] = CardColor.Green;
        standardColors[2] = CardColor.Blue;
        standardColors[3] = CardColor.Yellow;

        // loop throw array standardColors
        for (int i = 0; i < standardColors.Length; i++)
        {
            CardColor color = standardColors[i];
            
            // one 0 card for each color
            cards.Add(new Card(color, CardType.Number, 0));
            
            // 2 of 1-9 for each color
            for (int num = 1; num <= 9; num++)
            {
                cards.Add(new Card(color, CardType.Number, num));
                cards.Add(new Card(color, CardType.Number, num));
            }
            
            // two of action cards for each color
            for (int j = 0; j < 2; j++)
            {
                cards.Add(new Card(color, CardType.Skip));
                cards.Add(new Card(color, CardType.Reverse));
                cards.Add(new Card(color, CardType.DrawTwo));
            }
        } 
        
        // there are 4 wild cards (color palette) + 4 wild draw4s
        for (int i = 0; i < 4; i++)
        {
            cards.Add(new Card(CardColor.Wild, CardType.Wild));
            cards.Add(new Card(CardColor.Wild, CardType.WildDrawFour));
        }
    }
    
    public void Shuffle()
    {
        // Fisher-Yates shuffle algorithm 
        // YouTube: https://youtube.com/shorts/N4eN_sexrvU?si=Wf_NkiAUrPq-nxWM
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }
    
    public Card Draw()
    {
        // if there's no cards left -> null!
        if (cards.Count == 0) return null;
        
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
    
    public int CardsRemaining()
    {
        return cards.Count;
    }
}