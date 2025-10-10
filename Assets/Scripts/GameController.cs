using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] PumpkinLogic[] pumpkinArray;
    [SerializeField] private Deck deck;
    [SerializeField] private Player player;
    [SerializeField] private AI ai;
    [SerializeField] private GameEndScreen gameEndScreen;

    private List<PumpkinLogic> pumpkins;
    private List<string> pumpkinMessages = new List<string> { "Halfhealth", "-2health", "+2health", "Nullcards", "+1card", "-1card" };
    private List<Func<double, double>> pumpkinApplications = new List<Func<double, double>> { (t) => t == 0.5 ? 0.75 : t == -0.5 ? 1.5 : Math.Pow(0.5, t), (t) => -2 * t, (t) => 2 * t, (t) => 1.0, (t) => t, (t) => t };
    private int pumpkinIndex1;
    private int pumpkinIndex2;
    private Card p1Card;
    private Card p2Card;

    private List<PumpkinLogic> MakePumpkins()
    {
        List<PumpkinLogic> newPumpkins = new List<PumpkinLogic>();
        for (int i = 0; i < pumpkinArray.Length; i++)
        {
            PumpkinLogic currentPumpkin = this.pumpkinArray[i];
            currentPumpkin.pumpkinClicked += PumpkinChosen;
            newPumpkins.Add(currentPumpkin);
        }
        return newPumpkins;
    }

    void Start()
    {
        
        this.pumpkins = MakePumpkins();
        this.player.cardClicked += CardChosen;
        StartCoroutine(MainGame());
    }

    private IEnumerator MainGame()
    {
        this.deck.AllocateCards(this.player, this.ai);
        bool winner = false;
        int round = 1;
        while (!winner)
        {
            yield return PlayRound(round, this.player, this.ai);
            round++;
            if (this.player.GetScore() == 3 || this.ai.GetScore() == 3) winner = true;
            else
            {
                this.deck.Reset();
                this.deck.AllocateCards(this.player, this.ai);
            }
        }
        if (player.GetScore() == 3 && ai.GetScore() == 3) gameEndScreen.ShowEndScreen("You have drawn");
        else if (player.GetScore() == 3) gameEndScreen.ShowEndScreen("You win");
        else gameEndScreen.ShowEndScreen("You lose");
        yield return null;
    }

    private IEnumerator PlayRound(int round, Player player, AI ai)
    {
        Player player1;
        Player player2;
        if (round % 2 == 1)
        {
            player1 = player;
            player2 = ai;
        }
        else
        {
            player1 = ai;
            player2 = player;
        }
        player1.UpdateHealth(5);
        player2.UpdateHealth(5);
        bool roundEnd = false;
        int turnCounter = 1;
        while (!roundEnd)
        {
            AddPumpkins(turnCounter);
            RandomisePumpkins();
            yield return PlayNextTurn(player1, player2);
            roundEnd = player1.GetHealth() == 0 || player2.GetHealth() == 0;
            turnCounter++;
            player1.IncreaseCardsOwed();
            player2.IncreaseCardsOwed();
            this.deck.Take(player1, player2);
            this.p1Card.Used();
            this.p2Card.Used();
        }
        if (player2.GetHealth() == 0) player1.IncreaseScore();
        if (player1.GetHealth() == 0) player2.IncreaseScore();
        yield return new WaitForSeconds(2f);
        yield return ResetPumpkins();
    }

    private IEnumerator ResetPumpkins()
    {
        this.pumpkinMessages = new List<string> { "Halfhealth", "-2health", "+2health", "Nullcards", "+1card", "-1card" };
        this.pumpkinApplications = new List<Func<double, double>> { (t) => t == 0.5 ? 0.75 : t == -0.5 ? 1.5 : Math.Pow(0.5, t), (t) => -2 * t, (t) => 2 * t, (t) => 1.0, (t) => t, (t) => t };
        foreach (PumpkinLogic pumpkin in this.pumpkins)
        {
            pumpkin.Reset();
        }
        yield return null;
    }

    public void AddPumpkins(int currentTurn)
    {
        if (currentTurn == 2)
        {
            this.pumpkinMessages.Add("-2cards");
            this.pumpkinApplications.Add((t) => 2 * t);
        }
        if (currentTurn == 3)
        {
            this.pumpkinMessages.Add("-3health");
            this.pumpkinApplications.Add((t) => -3 * t);
        }
        if (currentTurn == 4)
        {
            this.pumpkinMessages.Add("Clearhand");
            this.pumpkinApplications.Add((t) => t);
        }
        if (currentTurn == 5)
        {
            this.pumpkinMessages.Add("Instantdeath");
            this.pumpkinApplications.Add((t) => -100 * t);
        }
    }


    public void RandomisePumpkins()
    {
        List<string> messages = new List<string>(this.pumpkinMessages);
        List<Func<double, double>> applications = new List<Func<double, double>>(this.pumpkinApplications);
        for (int i = 0; i < this.pumpkins.Count; i++)
        {
            int index = UnityEngine.Random.Range(0, messages.Count);
            string chosenString = messages[index];
            Func<double, double> chosenApplication = applications[index];
            this.pumpkins[i].AssignRole(chosenString, chosenApplication);
            messages.RemoveAt(index);
            applications.RemoveAt(index);
        }
    }


    public IEnumerator PlayNextTurn(Player player1, Player player2)
    {
        foreach (PumpkinLogic pumpkin in this.pumpkins)
        {
            pumpkin.EnableClicking();
        }
        List<PumpkinLogic> clonePumpkins = new List<PumpkinLogic>(this.pumpkins);
        if (player1.IsAi())
        {
            this.pumpkinIndex1 = player1.PickPumpkin(clonePumpkins);
            this.pumpkins[this.pumpkinIndex1].TriggerAnimation();
            this.pumpkins[this.pumpkinIndex1].DisableClicking();
        }
        else
        {
            this.pumpkinIndex1 = -1;
            yield return new WaitUntil(() => this.pumpkinIndex1 != -1);
            foreach (PumpkinLogic pumpkin in this.pumpkins)
            {
                pumpkin.DisableClicking();
            }

        }
        clonePumpkins.RemoveAt(this.pumpkinIndex1);
        yield return new WaitForSeconds(2f);
        if (player2.IsAi())
        {
            this.pumpkinIndex2 = this.pumpkins.IndexOf(clonePumpkins[player2.PickPumpkin(clonePumpkins)]);
            this.pumpkins[this.pumpkinIndex2].EnableClicking();
            this.pumpkins[this.pumpkinIndex2].TriggerAnimation();
            this.pumpkins[this.pumpkinIndex2].DisableClicking();
        }
        else
        {
            this.pumpkinIndex2 = -1;
            yield return new WaitUntil(() => this.pumpkinIndex2 != -1);
            foreach (PumpkinLogic pumpkin in this.pumpkins)
            {
                pumpkin.DisableClicking();
            }
        }
        yield return new WaitForSeconds(1f);
        if (player2.IsAi())
        {
            this.p2Card = player2.GetCard();
        }
        else
        {
            this.p2Card = null;
            player2.EnableCardClicking();
            yield return new WaitUntil(() => this.p2Card != null);
        }
        yield return new WaitForSeconds(1f);
        if (player1.IsAi())
        {
            this.p1Card = player1.GetCard();
        }
        else
        {
            this.p1Card = null;
            player1.EnableCardClicking();
            yield return new WaitUntil(() => this.p1Card != null);
        }
        yield return new WaitForSeconds(2f);
        CalculateResult(this.pumpkins[this.pumpkinIndex1], this.pumpkins[this.pumpkinIndex2], player1, player2);
        yield return new WaitForSeconds(2f);
    }

    public void CalculateResult(PumpkinLogic p1Pumpkin, PumpkinLogic p2Pumpkin, Player player1, Player player2)
    {
        int p1health = player1.GetHealth();
        int p2health = player2.GetHealth();
        bool nullified = false;
        if (p1Pumpkin.GetEffect() == "Nullcards") nullified = true;
        if (p2Pumpkin.GetEffect() == "Nullcards") nullified = true;
        p1health = p1Pumpkin.ApplyEffects(p1health, this.p1Card, this.p2Card, player1, nullified);
        p2health = p1Pumpkin.ApplyEffects(p2health, this.p2Card, this.p1Card, player2, nullified);
        p1health = p2Pumpkin.ApplyEffects(p1health, this.p1Card, this.p2Card, player1, nullified);
        p2health = p2Pumpkin.ApplyEffects(p2health, this.p2Card, this.p1Card, player2, nullified);
        player1.UpdateHealth(p1health);
        player2.UpdateHealth(p2health);
    }

    public void PumpkinChosen(PumpkinLogic chosenPumpkin)
    {
        if (this.pumpkinIndex1 == -1) this.pumpkinIndex1 = this.pumpkins.IndexOf(chosenPumpkin);
        else this.pumpkinIndex2 = this.pumpkins.IndexOf(chosenPumpkin);
    }

    public void CardChosen(Player playerChoosing, Card chosenCard)
    {
        playerChoosing.DisableCardClicking();
        if (this.p1Card == null) this.p1Card = chosenCard;
        else this.p2Card = chosenCard;
    }
}
