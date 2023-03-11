using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Constants
    readonly protected int WIN = 1, LOSE = 0, TIE=2;
    readonly protected int ATTACK = 1, DEFEND = 2, DODGE = 3, GRAB = 4, TRUMP = 5, SAFE = 6, NULL = -1, IDLE = 0;

    // GameObjects
    public Animator playerAnimator;
    public HealthBar playerHealthBar;
    public GameManager gameManager;
    public CombatManager combatManager;

    // CombatManager values
    public int playerNum, oppPlayerNum; //Player Numbers
    public int priority, oppPriority, outcome, result; public string action, oppAction;//Combat Manager Variables 

    //Character variables
    public int maxHealth;
    public string[] comboLog = new string[5];



    public void SetCharacter(int playerNum, int oppPlayerNum, int oppCharNum) { //Sets playerNums and directions
        gameObject.name = ("Player" + playerNum);
        if (playerNum == 2) {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        playerAnimator.SetInteger("Character", oppCharNum);
        this.playerNum = playerNum;
        this.oppPlayerNum = oppPlayerNum;
    }

    public int ActionEval(string oppAction, int oppPriority) { //Calls ActionEvals
        this.oppAction = oppAction;
        this.oppPriority = oppPriority;
        //Invoke(action + "Eval", 0f);
        SendMessage(action + "Eval");
        return outcome;
    }

    public void Action(int result) { //Calls Actions
        this.result = result;
        SendMessage(action);
        if (result != LOSE) {
            LogMove(action);
        } else {
            LogMove("Nothing");
        }
    }

    //-------------------------------------ATTACK-------------------------------------
    public void AttackPrep(int priority) {
        playerAnimator.SetTrigger("Attack");
        playerAnimator.SetInteger("AttackDegree", priority);
        action = "Attack";
        this.priority = priority;
    } public void AttackEval() {
        Debug.Log(playerNum + " Priority: " + priority);
        if (oppAction.Equals("Attack") && oppPriority >= priority) {
            outcome = IDLE;
        } else {
            outcome = ATTACK;
        }
    } public void Attack() {
        int damage = 0;
        int attackStr = priority;
        if (oppAction.Equals("Attack"))
            attackStr -= oppPriority;
        if (result == WIN) {
            if (gameManager.grabState) {
                damage = 20;
            } else {
                damage = attackStr * 5;
            }
            gameManager.Damage(oppPlayerNum, damage);
        }
        playerAnimator.SetTrigger("Action");
    }

    //-------------------------------------DEFEND-------------------------------------
    public void DefendPrep(int priority) {
        playerAnimator.SetTrigger("Defend");
        action = "Defend";
        this.priority = priority;
    } public void DefendEval() {
        outcome = DEFEND;
    } public void Defend() {
        if (gameManager.grabState == false) {
            if (result == WIN) {
                if (oppAction.Equals("Attack")) {
                    gameManager.Damage(oppPlayerNum, priority*3);
                } else {
                    Debug.Log("Counter!!!");
                    gameManager.Counter(playerNum);
                }
            }
            playerAnimator.SetTrigger("Action");
        }
    }

    //--------------------------------------DODGE--------------------------------------
    public void DodgePrep(int priority) {
        playerAnimator.SetTrigger("Dodge");
        action = "Dodge";
        this.priority = priority;
    } public void DodgeEval() {
        outcome = DODGE;
    } public void Dodge() {
        if (gameManager.grabState == false) {
            playerAnimator.SetTrigger("Action");
        }
    }
    

    //--------------------------------------GRAB--------------------------------------
    public void GrabPrep(int priority) {
        playerAnimator.SetTrigger("Grab");
        action = "Grab";
        this.priority = priority;
    } public void GrabEval() {
        outcome = GRAB;
    } public void Grab() {
        if (gameManager.grabState == false) {
            if (result == WIN) {
                gameManager.grabState = true;
                GrappleAnim();
                gameManager.grabTurn = 1;
                gameManager.Stun(oppPlayerNum, 1);
            }
            playerAnimator.SetTrigger("Action");
        }
    }
                //--------GRAPPLE-------
    public void GrappleAnim() {
        gameManager.HidePlayer(oppPlayerNum);
        playerAnimator.SetBool("Grapple", true);
        Debug.Log("Grapple TRUE");
    }

    //--------------------------------------OTHER--------------------------------------
    public void IdlePrep() {
        action = "Idle";
        this.priority = 0;
    } public void IdleEval() {
        outcome = IDLE;
    } public void Idle() {
    }

    public void NothingPrep() {
        action = "Nothing";
        this.priority = 0;
    } public void NothingEval() {
        outcome = IDLE;
    } public void Nothing() {
    }

    public void LogMove(string action) {
        for (int i = comboLog.Length - 1; i > 0; --i) {
            comboLog[i] = comboLog[i - 1];
        }
        comboLog[0] = action;
    } public bool ComboEval(string[] comboString) {
        bool combo = true;
        for(int i = 0; i < comboString.Length; i++) {
            if (!comboString[i].Equals(comboLog[i])) {
                combo = false;
            }
            //Debug.Log(i + " move: " + comboLog[i]);
        }
        //Debug.Log("Combo " + combo);
        return combo;
    }

    public void ResetCombat() {
        action = "Nothing";
        priority = 0;
        outcome = NULL;
        result = 0;
    }
}
