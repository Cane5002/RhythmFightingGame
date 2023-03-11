using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //CONSTANTS
    public static readonly int ATTACK = 1, DEFEND = 2, DODGE = 3, GRAB = 4, TRUMP = 5, SAFE = 6, NULL=-1, IDLE=0;

    //Variables
    public Character p1, p2;
    public string p1Action = "Nothing", p2Action = "Nothing";
    public int stun = 0;
    public GameManager gameManager;

    void Start() {
        gameManager = gameObject.GetComponent<GameManager>();
        p1 = gameManager.p1.GetComponent<Character>();
        p2 = gameManager.p2.GetComponent<Character>();

    }

    public void TryCombat() { //Called by GameManager
        p1Action = p1.action;
        p2Action = p2.action;
        if ((!p1Action.Equals("Nothing")) && (!p2Action.Equals("Nothing")) || (stun > 0 && ((!p1Action.Equals("Nothing")) || (!p2Action.Equals("Nothing"))))) {
            Debug.Log("EVALUATING");
            if (stun > 0)
                --stun;
            gameManager.evalComp = false;
            roundEval(p1, p2);
            p1.ResetCombat(); p2.ResetCombat(); //RESET
            gameManager.evalComp = true;

        } else {
            return;
        }
    }
    
    //---------------------------------------------------------


    //-------------------------------------------------------------------------------ROUND LOGIC-------------------------------------------------------------------------------
    public static void roundEval(Character p1, Character p2) { //Calls Character Actions -- Normal State
        int p1Result=0, p2Result=0;
        int p1Outcome = p1.ActionEval(p2.action, p2.priority);
        int p2Outcome = p2.ActionEval(p1.action, p1.priority);
        Debug.Log("p1: " + p1Outcome + "| p2: " + p2Outcome);
        if (p1Outcome == ATTACK) {
            if (p2Outcome == DEFEND || p2Outcome == DODGE || p2Outcome == TRUMP) {
                p1Result = 0; p2Result = 1;
            } else if (p2Outcome == SAFE) {
                p1Result = 2; p2Result = 2;
            } else {
                p1Result = 1; p2Result = 0;
            }
        } else if (p1Outcome == DEFEND) {
            if (p2Outcome == GRAB || p2Outcome == TRUMP) {
                p1Result = 0; p2Result = 1;
            } else if (p2Outcome == SAFE || p2Outcome == DEFEND || p2Outcome == IDLE) {
                p1Result = 2; p2Result = 2;
            } else {
                p1Result = 1; p2Result = 0;
            }
        } else if (p1Outcome == DODGE) {
            if (p2Outcome == DEFEND || p2Outcome == TRUMP) {
                p1Result = 0; p2Result = 1;
            } else if (p2Outcome == SAFE || p2Outcome == DODGE || p2Outcome == IDLE) {
                p1Result = 2; p2Result = 2;
            } else {
                p1Result = 1; p2Result = 0;
            }
        } else if (p1Outcome == GRAB) {
            if (p2Outcome == ATTACK || p2Outcome == DODGE || p2Outcome == TRUMP) {
                p1Result = 0; p2Result = 1;
            } else if (p2Outcome == SAFE || p2Outcome == GRAB) {
                p1Result = 2; p2Result = 2;
            } else {
                p1Result = 1; p2Result = 0;
            }
        } else if (p1Outcome == TRUMP) {
            if (p2Outcome == TRUMP || p2Outcome == SAFE) {
                p1Result = 2; p2Result = 2;
            } else {
                p1Result = 1; p2Result = 0;
            }
        } else if (p1Outcome == IDLE) {
            if (p2Outcome == IDLE || p2Outcome == DODGE || p2Outcome == DEFEND) {
                p1Result = 2; p2Result = 2;
            } else {
                p1Result = 0; p2Result = 1;
            }
        } else if (p1Outcome == SAFE) {
            p1Result = 2; p2Result = 2;
        }
        p1.gameObject.SendMessage("Action", p1Result);//p1.Action(p1Result);
        p2.gameObject.SendMessage("Action", p2Result);//p2.Action(p2Result);
    }

    public void grappleRoundEval() { //Calls Character Actions -- Grapple State

    }
}
