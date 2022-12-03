using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoop : MonoBehaviour
{
    public StateMachine stateMachine;
    public Player player;
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
        stateMachine.ChangeState("PlayerEndTurn");
    }
    void PlayerDraw()
    {
        while (player.numcards < 4) { player.DrawFromDeckToHand(1); }
        stateMachine.ChangeState("PlayerTurn");
    }
    void PlayerTurn()
    {
        Debug.Log("Player Turn");
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
        Debug.Log("Moving to opponent's turn");
        stateMachine.ChangeState("OpponentTurn");
    }
    void OpponentFirstTurn()
    {
        stateMachine.ChangeState("OpponentEndTurn");
    }
    void OpponentTurn()
    {
        Debug.Log("Opponent Turn");
        stateMachine.ChangeState("OpponentEndTurn");
    }
    void OpponentEndTurn()
    {
        stateMachine.ChangeState("PlayerDraw");
    }

    void AssembleStatemachine()
    {
        stateMachine = new StateMachine(PlayerFirstTurn, PlayerDraw, PlayerTurn, PlayerEndTurn, OpponentFirstTurn, OpponentTurn, OpponentEndTurn);
        stateMachine.ChangeState("PlayerFirstTurn");
    }
}
