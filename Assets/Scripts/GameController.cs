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

    private List<PumpkinLogic> MakePumpkins()
    {
        List<PumpkinLogic> newPumpkins = new List<PumpkinLogic>();
        List<string> pumpkinMessages = new List<string> { "Half health", "-2 health", "+2 health", "Nullify cards", "+1 card", "-1 card" };
        List<Func<double, double>> pumpkinApplications = new List<Func<double, double>> { (t) => t == 0.5 ? 0.75 : t == -0.5 ? 1.5 : Math.Pow(0.5, t), (t) => -2 * t, (t) => 2 * t, (t) => 1.0, (t) => t, (t) => t };
        for (int i = 0; i < 1/*pumpkinMessages.Count*/; i++)
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
        StartCoroutine(MainGame());
    }

    private IEnumerator MainGame()
    {
        this.deck.AllocateCards(this.player, this.ai);
        //System.out.println("Start Game\n");
        bool winner = false;
        int round = 1;
        while (round < 2/*!winner*/) {
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
        /*Player player1;
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
        int turnCounter = 1;*/
        Debug.Log("Successfull execution");
        return;
    }

    public void PumpkinChosen(PumpkinLogic chosenPumpkin)
    {

    }
}
