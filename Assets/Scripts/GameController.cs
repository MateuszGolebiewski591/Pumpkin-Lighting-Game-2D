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

    private List<PumpkinLogic> pumpkins;
    private List<string> pumpkinMessages = new List<string> { "Halfhealth", "-2health", "+2health", "Nullcards", "+1card", "-1card" };
    private List<Func<double, double>> pumpkinApplications = new List<Func<double, double>> { (t) => t == 0.5 ? 0.75 : t == -0.5 ? 1.5 : Math.Pow(0.5, t), (t) => -2 * t, (t) => 2 * t, (t) => 1.0, (t) => t, (t) => t };


    private List<PumpkinLogic> MakePumpkins()
    {
        List<PumpkinLogic> newPumpkins = new List<PumpkinLogic>();
        for (int i = 0; i < pumpkinArray.Length; i++)
        {
            PumpkinLogic currentPumpkin = this.pumpkinArray[i];
            currentPumpkin.pumpkinClicked += PumpkinChosen;
            currentPumpkin.AssignRole(pumpkinMessages[i], pumpkinApplications[i]);
            newPumpkins.Add(currentPumpkin);
        }
        return newPumpkins;
    }

    void Start()
    {
        this.pumpkins = MakePumpkins();
        StartCoroutine(MainGame());
    }

    private IEnumerator MainGame()
    {
        this.deck.AllocateCards(this.player, this.ai);
        bool winner = false;
        int round = 1;
        while (round < 2/*!winner*/)
        {
            //System.out.println("Scores | Player : " + player.getScore() + " | AI : " + ai.getScore());
            PlayRound(round, this.player, this.ai);
            round++;
            /*if (this.player.GetScore()==3 || this.ai.GetScore()==3) winner = true;
            else {
                this.deck.Reset();
                this.deck.AllocateCards(this.player, this.ai);
            }*/
        }
        /*System.out.println("Scores | Player : " + player.getScore() + " | AI : " + ai.getScore());
        if (player.getScore()==3 && ai.getScore() == 3) System.out.println("It's a draw!");
        else if (player.getScore() == 3) System.out.println("You win!");
        else System.out.println("You lost!");*/

        yield return null;
    }

    private void PlayRound(int round, Player player, AI ai)
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
        player1.updateHealth(5);
        player2.updateHealth(5);
        bool roundEnd = false;
        int turnCounter = 1;
        while (turnCounter < 2 /*!roundEnd*/)
        {
            AddPumpkins(turnCounter);
            //System.out.println("\nRound " + round + ", Turn " + turnCounter);
            //System.out.println("Player Health " + player.getHealth() + "| AI Health " + ai.getHealth() + "| Deck size " + this.deck.size() + "\n");
            //List<Pumpkin> intoTurns = RandomisePumpkins();
            //roundEnd = PlayNextTurn(player1, player2, intoTurns);
            turnCounter++;
            player1.IncreaseCardsOwed();
            player2.IncreaseCardsOwed();
            this.deck.Take(player1, player2);
        }
        if (player2.GetHealth() == 0) player1.IncreaseScore();
        if (player1.GetHealth() == 0) player2.IncreaseScore();
        ResetPumpkins();


        Debug.Log("Successfull execution");
        return;
    }

    private void ResetPumpkins()
    {
        this.pumpkinMessages = new List<string> { "Halfhealth", "-2health", "+2health", "Nullcards", "+1card", "-1card" };
        this.pumpkinApplications = new List<Func<double, double>> { (t) => t == 0.5 ? 0.75 : t == -0.5 ? 1.5 : Math.Pow(0.5, t), (t) => -2 * t, (t) => 2 * t, (t) => 1.0, (t) => t, (t) => t };
        for (int i = 0; i < pumpkins.Count; i++)
        {
            this.pumpkins[i].AssignRole(pumpkinMessages[i], pumpkinApplications[i]);
        }
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
        for (int i = 0; i < 5; i++)
        {
            int index = UnityEngine.Random.Range(0, messages.Count);
            string chosenString = messages[index];
            Func<double, double> chosenApplication = applications[index];
            this.pumpkins[i].AssignRole(chosenString, chosenApplication);
            messages.RemoveAt(index);
            applications.RemoveAt(index);
        }
    }

    //The function below will have the waiting for player / ai input

    /*public boolean PlayNextTurn(Player player1, Player player2, List<Pumpkin> pumpkinsOrigin) {
        List<Pumpkin> pumpkins = new ArrayList<>(pumpkinsOrigin);
        int chosen = player1.pickPumpkin(pumpkins)-1;
        //Pumpkin p1Pumpkin = pumpkins.get(chosen);
        pumpkins.RemoveAt(chosen);
        chosen = player2.PickPumpkin(pumpkins)-1;
        Pumpkin p2Pumpkin = pumpkins[chosen];
        pumpkins.RemoveAt(chosen);
        List<Card> p2Card = player2.GetCard();
        List<Card> p1Card = player1.GetCard();
        CalculateResult(p1Pumpkin, p2Pumpkin, p1Card, p2Card, player1, player2);
        return player1.getHealth() == 0 || player2.getHealth() == 0;
    }

    public void CalculateResult(Pumpkin p1Pumpkin, Pumpkin p2Pumpkin, List<Card> p1Card, List<Card> p2Card, Player player1, Player player2) {
        int p1health = player1.GetHealth();
        int p2health = player2.GetHealth();
        bool nullified = false;
        if (p1Pumpkin.GetEffect() == "Nullcards") nullified = true;
        if (p2Pumpkin.GetEffect() == "Nullcards") nullified = true;
        p1health = p1Pumpkin.ApplyEffects(p1health, p1Card, p2Card, player1, nullified);
        p2health = p1Pumpkin.ApplyEffects(p2health, p2Card, p1Card, player2, nullified);
        p1health = p2Pumpkin.ApplyEffects(p1health, p1Card, p2Card, player1, nullified);
        p2health = p2Pumpkin.ApplyEffects(p2health, p2Card, p1Card, player2, nullified);
        player1.UpdateHealth(p1health);
        player2.UpdateHealth(p2health);
    }*/

    //The funtion above calls UpdateHealth, which in player, needs to be connected to the health UI unit which needs to be added.

    public void PumpkinChosen(PumpkinLogic chosenPumpkin)
    {

    }

    public void CardChosen(Player playerChoosing, Card chosenCard)
    {
        
    }
}
