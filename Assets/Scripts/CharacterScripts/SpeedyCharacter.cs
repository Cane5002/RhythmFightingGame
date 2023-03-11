using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedyCharacter : Character
{
    public GameObject Bar;
    public BarMeter speedBar;
    public int barCharge = 0;

    //COMBOS
    public string[] combo1 = { "Dodge", "Dodge"};
    public string[] combo2 = { "Dodge", "Defend", "Dodge"};
    public string[] combo3 = { "Dodge", "Dodge", "Dodge"};

    void Start() {
        gameManager = GameManager.FindObjectOfType<GameManager>();
        //playerAnimator = gameObject.GetComponent<Animator>();

        //SET HEALTH BAR
        playerHealthBar = GameObject.Find("p" + playerNum + "HealthBar").GetComponent<HealthBar>();
        playerHealthBar.SetMaxHealth(maxHealth);

        //SET SPEED BAR
        speedBar = Instantiate(Bar, playerHealthBar.transform.position + new Vector3(0, -25), Quaternion.identity).GetComponent<BarMeter>();
        speedBar.SetBar(5, 0, "p" + playerNum + "SpeedBar", playerNum, 182);

        Debug.Log("Starting Speedy");
    }

    //-------------------------------------ATTACK-------------------------------------
    new public void AttackPrep(int priority) {
        action = "Attack";
        if (barCharge >= 3) {
            ++priority;
            playerAnimator.SetTrigger("SpecialAttack");
        } else {
            playerAnimator.SetTrigger("Attack");
            playerAnimator.SetInteger("AttackDegree", priority);
        }
        if (ComboEval(combo3)) {
            ++priority;
        }
        this.priority = priority;
    } new public void Attack() {
        int damage = 0;
        int attackStr = priority;
        if (oppAction.Equals("Attack"))
            attackStr -= oppPriority;
        if (barCharge >=3) {
            damage += (barCharge - 3) * 10;
            Empty();
        }
        if (ComboEval(combo1) && !ComboEval(combo3)) {
            damage += 5;
        }
        if (result == WIN) {
            if (gameManager.grabState) {
                damage = 20;
            }
            else {
                damage += attackStr * 5;
            }
            gameManager.Damage(oppPlayerNum, damage);
        }
        playerAnimator.SetTrigger("Action");
    }

    //--------------------------------------DODGE--------------------------------------
    new public void DodgeEval() {
        if (barCharge == 5) {
            playerAnimator.SetTrigger("Stun");
            outcome = IDLE;
            action = "Idle";
            Empty();
        } else {
            outcome = DODGE;
        }
    } new public void Dodge() {
        if (gameManager.grabState == false) {
            if (result != LOSE) {
                Charge();
            }
            playerAnimator.SetTrigger("Action");
        }
    }

    //--------------------------------------GRAB--------------------------------------
    new public void GrabEval() {
        if (ComboEval(combo2) && oppAction.Equals("Dodge")) {
            outcome = TRUMP;
        } else {
            outcome = GRAB;
        }
    }

    //-----------------------------------SPEED-BAR-----------------------------------
    public void Charge() {
        barCharge++;
        speedBar.SetFill(barCharge);
    }
    public void Empty() {
        barCharge = 0;
        speedBar.SetFill(0);
    }
}
