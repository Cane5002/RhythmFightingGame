using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceCharacter : Character
{
    public string stance;
    public Material noAura, attackAura, defenseAura, grabAura, superAura;
    void Start() {
        gameManager = GameManager.FindObjectOfType<GameManager>();

        //SET HEALTH BAR
        playerHealthBar = GameObject.Find("p" + playerNum + "HealthBar").GetComponent<HealthBar>();
        playerHealthBar.SetMaxHealth(maxHealth);
        
        
        SetStance("Attack");

        Debug.Log("Starting SandBag");
    }


    //-------------------------------------ATTACK-------------------------------------
    new public void AttackPrep(int priority) {
        playerAnimator.SetTrigger("Attack");
        playerAnimator.SetInteger("AttackDegree", priority);
        action = "Attack";
        if (stance.Equals("Defend")) {
            priority++;
        }
        this.priority = priority;
    } new public void AttackEval() {
        if (stance.Equals("Super")) {
            outcome = TRUMP;
        } else if (oppAction.Equals("Attack") && oppPriority >= priority) {
            outcome = IDLE;
        } else {
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
            } else {
                damage = attackStr * 5;
            }
            if (stance.Equals("Attack") || stance.Equals("Super")) {
                gameManager.Damage(playerNum, -10);
            } if (stance.Equals("Grab") || stance.Equals("Super")) {
                damage += 10;
            } if (stance.Equals("Defend")) {
                damage -= 5;
            }
            gameManager.Damage(oppPlayerNum, damage);
        }
        playerAnimator.SetTrigger("Action");
    }

    //-------------------------------------OTHER-------------------------------------
    new public void Action(int result) { //Calls Actions
        this.result = result;
        SendMessage(action);
        if (result != LOSE) {
            LogMove(action);
        }
        else {
            LogMove("Nothing");
        }
        //Set Stance
        stance = action;
        if (ComboEval()) {
            stance = "Super";
        }
        if (result == LOSE) {
            stance = "Nothing";
        }
        SetStance(stance);
    }

    //-------------------------------------STANCE-------------------------------------
    public bool ComboEval() {
        bool def = false, atk = false, grb = false, combo = false;
        for (int i = 0; i < 3; i++) {
            if (comboLog[i].Equals("Attack")) {
                atk = true;
            } else if (comboLog[i].Equals("Defend")) {
                def = true;
            } else if (comboLog[i].Equals("Grab")) {
                grb = true;
            }
            //Debug.Log(i + " move: " + comboLog[i]);
        }
        if (def && atk && grb) {
            combo = true;
        }
        //Debug.Log("Combo " + combo);
        return combo;
    }

    public void SetStance(string stance) {
        Material aura = noAura;
        if (stance.Equals("Attack")) {
            aura = attackAura;
        } else if (stance.Equals("Defend")) {
            aura = defenseAura;
        } else if (stance.Equals("Grab")) {
            aura = grabAura;
        } else if (stance.Equals("Super")) {
            aura = superAura;
        }
        gameObject.GetComponent<SpriteRenderer>().material = aura;
    }

}
