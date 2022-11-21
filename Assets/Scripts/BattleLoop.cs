using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoop : MonoBehaviour
{
    public StateMachine stateMachine;
    Board board;

    private void Awake()
    {
        AssembleStatemachine();

    }

    void Update()
    {
        stateMachine.Execute();
    }
    void PlayerFirstTurn()
    {
        stateMachine.ChangeState("PlayerEndTurn");
    }
    void PlayerDraw()
    {
        stateMachine.ChangeState("PlayerTurn");
    }
    void PlayerTurn()
    {
        stateMachine.ChangeState("PlayerEndTurn");
    }
    void PlayerEndTurn()
    {
        stateMachine.ChangeState("OpponentTurn");
    }
    void OpponentTurn()
    {
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
