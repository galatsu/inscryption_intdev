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
    void PlayerFirstTurn()
    {
        player.DrawFromDeckToHand(4);
        stateMachine.ChangeState("OpponentTurn");
    }
    void PlayerDraw()
    {
        while (player.numcards < 4) { player.DrawFromDeckToHand(1); }
        Debug.Log("Player Turn");
        player.currentcost += 1;
        stateMachine.ChangeState("PlayerTurn");
    }
    void PlayerTurn()
    {
        player.playerturn = true;
        if (player.playerturn == false)
        {
            stateMachine.ChangeState("PlayerEndTurn");
        }
        player.stateMachine.ChangeState("CanSelectCardFromHand");
    }
    void PlayerEndTurn()
    {
        player.stateMachine.ChangeState("CantSelectCard");
        Debug.Log("Opponent Turn");
        stateMachine.ChangeState("OpponentTurn");
    }
    void OpponentTurn()
    {
        opponent.currentcost += 1;
        stateMachine.ChangeState("OpponentEndTurn");
    }
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
