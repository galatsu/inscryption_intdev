using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoop : MonoBehaviour
{
    public StateMachine stateMachine;
    public Player player;
    public Opponent opponent;
    public Board board;

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
        player.DrawFromDeckToHand(3);
        stateMachine.ChangeState("OpponentFirstTurn");
    }
    //if we don't have four cards, add cards until we do, then enable picking cards
    void PlayerDraw()
    {
        player.DrawFromDeckToHand(1);
        Debug.Log("Player Turn");
        player.currentcost = 1;
        stateMachine.ChangeState("PlayerTurn");
        player.playerturn = true;
        player.stateMachine.ChangeState("CanSelectCardFromHand");
    }
    //stay paused on this until we give the signal(defined in Player) to move forward
    void PlayerTurn()
    {
        if (player.playerturn == false)
        {
            stateMachine.ChangeState("PlayerEndTurn");
        }
    }
    //ending our turn; make it so we can't pick anything, then change to the opponent's turn
    void PlayerEndTurn()
    {
        player.currentcost = 0;
        player.stateMachine.ChangeState("CantSelectCard");
        board.PlayerAttacks();
        Debug.Log("Opponent Turn");
        stateMachine.ChangeState("OpponentTurn");
    }
    //draw four to the opponent; then immediately transition into the opponent playing an enemy card
    void OpponentFirstTurn()
    {
        opponent.DrawFromDeckToHand(3);
        stateMachine.ChangeState("OpponentEndTurn");
    }
    //the opponent plays a card
    void OpponentTurn()
    {
        opponent.DrawFromDeckToHand(1);
        board.OpponentsAdvance();
        opponent.currentcost = 1;
        //if the bool in board script says that there is an empty slot for the opponent to play a card in, the opponent picks and plays a card
        if (board.CheckIfLanesAreFull() == true)
        {
            opponent.PickAndPlayCard();
        }
        stateMachine.ChangeState("OpponentEndTurn");
    }
    //end opponent's turn
    void OpponentEndTurn()
    {
        opponent.currentcost = 0;
        board.OpponentAttacks();
        stateMachine.ChangeState("PlayerDraw");
    }

    void AssembleStatemachine()
    {
        stateMachine = new StateMachine(PlayerFirstTurn, PlayerDraw, PlayerTurn, PlayerEndTurn, OpponentFirstTurn, OpponentTurn, OpponentEndTurn);
        stateMachine.ChangeState("PlayerFirstTurn");
    }
}
