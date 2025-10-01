using System;
using UnityEngine;
using System.Collections.Generic;

public class PumpkinLogic : MonoBehaviour
{
    private Pumpkin pumpkin;
    private Board board;
    private string effect;
    private Func<double, double> application;

    private void Awake()
    {
        pumpkin = GetComponentInChildren<Pumpkin>();
        board = GetComponentInChildren<Board>();
    }

    public void AssignRole(string givenEffect, Func<double, double> givenApplication)
    {
        this.effect = givenEffect;
        this.application = givenApplication;
    }

    public string GetEffect()
    {
        return this.effect;
    }

    public int ApplyEffects()
    {
        return 1;
    }
    
    public int applyEffects(int playerHealth, List<Card> cards, List<Card> otherCards, Player player, bool nullified) {
        if (this.effect == "Nullify cards") return playerHealth;
        foreach (Card card in cards) {
            if ((card.GetCardName() == "Healthy Bone") && !nullified) return playerHealth;
        }
        foreach (Card card in otherCards) {
            if ((card.GetCardName() == "Bad Bone") && !nullified) return playerHealth;
        }
        double mul = 1.0;
        if (!nullified) {
            foreach (Card card in cards) {
                mul = card.Apply(mul);
            }
            foreach (Card card in otherCards) {
                mul = card.Apply(mul);
            }
        }
        double numEffect = this.application(mul);
        switch (this.effect) {
            case "Half health": return (int)(playerHealth * numEffect);
            case "+1 card": AddOrTakeCard(player, numEffect); return playerHealth;
            case "-1 card": AddOrTakeCard(player, -numEffect); return playerHealth;
            case "-2 card": AddOrTakeCard(player, -numEffect); return playerHealth;
            case "Clear hand": {
                if (mul >= 0.0) player.ClearCards();
                if (mul < 0.0) player.IncreaseCardsOwed(5);
                return playerHealth;
            }
            default : return playerHealth + (int)numEffect;
        }
    }


    public void AddOrTakeCard(Player player, double numEffect)
    {
        double i = 1.0;
        if (numEffect < 0.0)
        {
            i = -1.0;
            while (i >= numEffect)
            {
                player.UseRandomCard();
                i--;
            }
        }
        else
        {
            while (i <= numEffect)
            {
                player.IncreaseCardsOwed();
                i++;
            }
        }
    }
}
