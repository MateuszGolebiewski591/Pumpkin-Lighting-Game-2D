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

    public List<Card> GetCard()
    {
        if (base.handSize == 0) return null;
        int chosen = UnityEngine.Random.Range(0, base.hand.Count);
        Card chosenCard = base.hand[chosen];
        base.hand.RemoveAt(chosen);
        base.handSize--;
        return new List<Card> { chosenCard };
    }

    public override bool IsAi()
    {
        return true;
    }
    
}
