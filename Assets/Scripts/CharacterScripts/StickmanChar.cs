using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanChar : Character
{

    public bool bonus = false;
    public int attackRun = 0;
    public GameObject fireball;
    public int direction = 1;

    //COMBOS
    public string[] combo1 = { "Attack", "Attack"};

    void Start() {
        gameManager = GameManager.FindObjectOfType<GameManager>();
        //playerAnimator = gameObject.GetComponent<Animator>();
        //SET HEALTH BAR
        playerHealthBar = GameObject.Find("p" + playerNum + "HealthBar").GetComponent<HealthBar>();
        playerHealthBar.SetMaxHealth(maxHealth);
        //FIREBALL
        if (playerNum == 2) {
            fireball.transform.GetComponent<SpriteRenderer>().flipX = true;
        }
        Debug.Log("Starting Stickman");
    }

    //-------------------------------------ATTACK-------------------------------------
    new public void AttackPrep(int priority) {
        if (ComboEval(combo1)) {
            playerAnimator.SetTrigger("SpecialAttack");
        } else {
            playerAnimator.SetTrigger("Attack");
            playerAnimator.SetInteger("AttackDegree", priority);
        }
        action = "Attack";
        this.priority = priority;
    } new public void AttackEval() {
        if (oppAction.Equals("Attack") && oppPriority >= priority) {
            outcome = IDLE;
            if (ComboEval(combo1)) {
                outcome = SAFE;
            }
        } else if (oppAction.Equals("Defend") && ComboEval(combo1)) {
            outcome = SAFE;
        }
        else {
            outcome = ATTACK;
        }
    } new public void Attack() {
        int damage = 0;
        int attackStr = priority;
        if (oppAction.Equals("Attack"))
            attackStr -= oppPriority;
        if (result == WIN) {
            if (gameManager.grabState) {
                damage = 20;
                if (bonus) {
                    bonus = false;
                    damage += 10;
                }
            }
            else {
                damage = attackStr * 5;
            }
            damage += attackRun * 5;
            gameManager.Damage(oppPlayerNum, damage);
        } if (result != LOSE) {
            if (priority >= 3) {
                LogMove("Attack");
            } 
            ++attackRun;
        } else {
            attackRun = 0;
        }
        playerAnimator.SetTrigger("Action");
    }


    //-------------------------------------DEFEND-------------------------------------
    new public void Defend() {
        if (gameManager.grabState == false) {
            if (result == WIN) {
                if (oppAction.Equals("Attack")) {
                    gameManager.Damage(oppPlayerNum, priority * 3);
                    if (priority >= 3) {
                        gameManager.Stun(oppPlayerNum, 1);
                    }
                }
                else {
                    gameManager.Counter(playerNum);
                }
            }
            attackRun = 0;
            playerAnimator.SetTrigger("Action");
        }
    }

    //--------------------------------------GRAB--------------------------------------
    new public void Grab() {
        if (gameManager.grabState == false) {
            if (result == WIN) {
                gameManager.grabState = true;
                GrappleAnim();
                gameManager.grabTurn = 1;
                gameManager.Stun(oppPlayerNum, 1);
                if (priority >= 3) {
                    bonus = true;
                }
            }
            playerAnimator.SetTrigger("Action");
            attackRun = 0;
        }
    }

    //--------------------------------------DODGE--------------------------------------
    new public void DodgeEval() {
        if (priority >= 3 && oppAction.Equals("Defend")) {
            outcome = TRUMP;
        } else {
            outcome = DODGE;
        }
        attackRun = 0;
    }

    //--------------------------------------OTHER--------------------------------------
    new public void Idle() {
        attackRun = 0;
    }
    new public void Action(int result) { //Calls Actions
        this.result = result;
        SendMessage(action);
        if (result != LOSE) {
            if (ComboEval(combo1)) {
                LogMove("Nothing");
            } else {
                LogMove(action);
            }
        }
        else {
            LogMove("Nothing");
        }
    }

    public void Fireball() {
        if (playerNum == 2) {
            direction = -1;
        }
        Instantiate(fireball, gameObject.transform.position + new Vector3 (direction*0.42f,1.52f,.1f), Quaternion.identity).GetComponent<Fireball>().SetFireball(playerNum, oppPlayerNum);
    }



}
