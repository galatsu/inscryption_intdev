using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoop : MonoBehaviour
{
    public StateMachine stateMachine;
    public Player player;
    public Opponent opponent;
    Board board;

    private void Awake()
    {
        AssembleStatemachine();
    }

    void Update()
    {
        this.stateMachine.Execute();
    }
    //draw four to our card then move on to opponent's turn
    void PlayerFirstTurn()
    {
        player.DrawFromDeckToHand(4);
        stateMachine.ChangeState("OpponentTurn");
    }
    //if we don't have four cards, add cards until we do, then enable picking cards
    void PlayerDraw()
    {
        while (player.numcards < 4) { player.DrawFromDeckToHand(1); }
        Debug.Log("Player Turn");
        player.currentcost += 1;
        stateMachine.ChangeState("PlayerTurn");
        player.stateMachine.ChangeState("CanSelectCardFromHand");
    }
    //stay paused on this until we give the signal(defined in Player) to move forward
    void PlayerTurn()
    {
        player.playerturn = true;
        if (player.playerturn == false)
        {
            stateMachine.ChangeState("PlayerEndTurn");
        }
    }
    //ending our turn; make it so we can't pick anything, then change to the opponent's turn
    void PlayerEndTurn()
    {
        player.stateMachine.ChangeState("CantSelectCard");
        Debug.Log("Opponent Turn");
        stateMachine.ChangeState("OpponentTurn");
    }
    //the opponent plays a card
    void OpponentTurn()
    {
        opponent.currentcost += 1;
        stateMachine.ChangeState("OpponentEndTurn");
    }
    //end opponent's turn
    void OpponentEndTurn()
    {
        stateMachine.ChangeState("PlayerDraw");
    }

    void AssembleStatemachine()
    {
        stateMachine = new StateMachine(PlayerFirstTurn, PlayerDraw, PlayerTurn, PlayerEndTurn, OpponentTurn, OpponentEndTurn);
        stateMachine.ChangeState("PlayerFirstTurn");
    }
}
