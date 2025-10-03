using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
    protected List<Card> hand = new List<Card>();
    protected int handSize = 0;
    [SerializeField] private int health = 5;
    private int score = 0;
    private int cardsOwed = 0;

    public int GetHealth() { return this.health; }
    public void IncreaseScore() { this.score++; }
    public int GetScore() { return this.score; }
    public void IncreaseCardsOwed() { this.cardsOwed++; }
    public void IncreaseCardsOwed(int increment) { this.cardsOwed += increment; }

    public int GetCardsOwed()
    {
        int owed = this.cardsOwed;
        this.cardsOwed = 0;
        return Math.Min(5 - this.handSize, owed);
    }

    public void UseRandomCard()
    {
        if (this.handSize > 0)
        {
            this.handSize--;
            this.hand.RemoveAt(UnityEngine.Random.Range(0, this.hand.Count));
        }
    }

    public void ClearCards()
    {
        this.hand = new List<Card>();
        handSize = 0;
    }

    public bool AddCard(Card card)
    {
        if (handSize < 5)
        {
            this.hand.Add(card);
            this.handSize++;
            return true;
        }
        return false;
    }
    
    public void updateHealth(int newHealth) {
        if (newHealth > 5) this.health = 5;
        else if (newHealth < 0) this.health = 0;
        else this.health = newHealth;
    }
}
