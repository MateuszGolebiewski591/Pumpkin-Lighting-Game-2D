using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
    protected List<Card> hand = new List<Card>();
    protected int handSize = 0;
    [SerializeField] private int health = 5;
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private ScoreText scoreText;
    private int score = 0;
    private int cardsOwed = 0;
    public event Action<Player, Card> cardClicked;

    public int GetHealth() { return this.health; }
    public void IncreaseScore()
    {
        this.score++;
        this.scoreText.UpdateText(!IsAi(), this.score);
    }
    void Start()
    {
        this.scoreText.UpdateText(!IsAi(), 0);
    }
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
            int index = UnityEngine.Random.Range(0, this.hand.Count);
            Card card = this.hand[index];
            if (!IsAi()) card.Dispose();
            else card.Used();
            this.hand.RemoveAt(index);
            if (!IsAi())
            {
                for (int i = 0; i < this.handSize; i++)
                {
                    this.hand[i].Shift(i + 1);
                }
            }
            
        }
    }

    public void ClearCards()
    {
        foreach (Card card in this.hand)
        {
            if (!IsAi()) card.Dispose();
            else card.Used();
        }
        this.hand = new List<Card>();
        handSize = 0;
    }

    public virtual bool AddCard(Card card)
    {
        if (handSize < 5)
        {
            this.hand.Add(card);
            this.handSize++;
            card.AssumePosition(handSize);
            card.SetPlayer(this);
            return true;
        }
        return false;
    }

    public void UpdateHealth(int newHealth)
    {
        if (newHealth > 5) this.health = 5;
        else if (newHealth < 0) this.health = 0;
        else this.health = newHealth;
        this.healthbar.UpdateHealth(this.health);
    }

    public virtual bool IsAi()
    {
        return false;
    }

    public virtual int PickPumpkin(List<PumpkinLogic> pumpkins)
    {
        return -1;
    }

    public void HandleClick(Card clickedCard)
    {
        int index = this.hand.IndexOf(clickedCard);
        this.hand.RemoveAt(index);
        this.handSize--;
        for (int i = 0; i < this.handSize; i++)
        {
            this.hand[i].Shift(i+1);
        }
        this.cardClicked?.Invoke(this, clickedCard);
    }

    public void DisableCardClicking()
    {
        foreach (Card card in this.hand)
        {
            card.DisableClicking();
        }
    }

    public void EnableCardClicking()
    {
        foreach (Card card in this.hand)
        {
            card.EnableClicking();
        }
    }

    public virtual Card GetCard()
    {
        return null;
    }
}
