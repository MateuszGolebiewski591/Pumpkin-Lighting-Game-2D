using UnityEngine;
using System.Collections.Generic;
using System;

public class AI : Player
{
    public override int PickPumpkin(List<PumpkinLogic> pumpkins)
    {
        int chosen = UnityEngine.Random.Range(0, pumpkins.Count);
        return chosen;
    }

    public override Card GetCard()
    {
        if (base.handSize == 0) return null;
        int chosen = UnityEngine.Random.Range(0, base.hand.Count);
        Card chosenCard = base.hand[chosen];
        base.hand.RemoveAt(chosen);
        base.handSize--;
        chosenCard.MoveTo(new Vector3(6f, 2.1f, -2f));
        return chosenCard;
    }

    public override bool IsAi()
    {
        return true;
    }

    public override bool AddCard(Card card)
    {
        if (handSize < 5)
        {
            this.hand.Add(card);
            this.handSize++;
            card.AiPosition();
            return true;
        }
        return false;
    }
    
}
