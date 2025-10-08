using System;
using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [SerializeField] private Card[] cards;
    private List<Card> deck; 
    private int deckSize;

    void Awake()
    {
        this.deck = NewDeck();
        this.deckSize = this.deck.Count;
    }


    public List<Card> NewDeck()
    {

        List<Func<double, double>> applications = new List<Func<double, double>> { (t) => t, (t) => t, (t) => 2 * t, (t) => 0.5 * t, (t) => -t };
        List<Card> cardDeck = new List<Card>();
        for (int i = 0; i < cards.Length; i++)
        {
            Card card = cards[i];
            card.AssignRole(applications[i / 6]);
            card.ResetCard();
            cardDeck.Add(card);
        }

        return cardDeck;
    }

    public int Size() { return this.deckSize; }

    public void AllocateCards(Player player, Player ai) {
        
        player.ClearCards();
        ai.ClearCards();
        
        for (int i = 0; i < 5; i++)
        {
            if (this.deckSize > 1)
            {
                int chosen = UnityEngine.Random.Range(0, this.deckSize);
                if (player.AddCard(this.deck[chosen]))
                {
                    this.deck.RemoveAt(chosen);
                    this.deckSize--;
                }
                chosen = UnityEngine.Random.Range(0, this.deckSize);
                if (ai.AddCard(this.deck[chosen]))
                {
                    this.deck.RemoveAt(chosen);
                    this.deckSize--;
                }
            }
        }
    }

    public void Take(Player player1, Player player2) {
        bool finished = false;
        int player1CardsOwed = player1.GetCardsOwed();
        int player2CardsOwed = player2.GetCardsOwed();
        while (!finished) {
            if (this.deckSize == 0 ||(player1CardsOwed > 0 && player2CardsOwed > 0 && this.deckSize == 1)) finished = true;
            else {
                player1CardsOwed = GiveCard(player1, player1CardsOwed);
                player2CardsOwed = GiveCard(player2, player2CardsOwed);
                if (player1CardsOwed == player2CardsOwed && player2CardsOwed == 0) finished = true;
            }
        }
    }

    public int GiveCard(Player player, int cardsOwed) {
        if (this.deckSize <= 0) return 0;
        if (cardsOwed > 0) {
            int chosen = UnityEngine.Random.Range(0, this.deckSize);
            if (player.AddCard(this.deck[chosen])) {
                this.deck.RemoveAt(chosen);
                this.deckSize--;
                return cardsOwed - 1;
            }
            return 0;
        }
        return 0;
    }


    public void Reset()
    {
        this.deck = NewDeck();
        this.deckSize = this.deck.Count;
    }
    
}
