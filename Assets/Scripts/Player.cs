using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // Variables
    public Animator playerAnimator;
    public GameManager gameManager;
    public int characterNum, opponentNum;
    List<Object> characters = new List<Object>();
    //public Object healthBar = new HealthBar();

    // Start is called before the first frame update
    void Start() {
        gameManager = GameManager.FindObjectOfType<GameManager>();
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.SetInteger("Character", opponentNum);
        //healthBar = gameObject.AddComponent(healthBar.GetType());
    }

    // Update is called once per frame
    void Update() {
    }
    /*
    public void AttackAnim(int attackDegree) {
        playerAnimator.SetTrigger("Attack");
        playerAnimator.SetInteger("AttackDegree", attackDegree);
    }

    public void DefendAnim() {
        playerAnimator.SetTrigger("Defend");
    }

    public void GrabAnim() {
        playerAnimator.SetTrigger("Grab");
    }

    public void DodgeAnim() {
        playerAnimator.SetTrigger("Dodge");
    }
    */
    public void GrappleAnim() {
        gameManager.HidePlayer(opponentNum);
        playerAnimator.SetBool("Grapple", true);
        Debug.Log("Grapple TRUE");
        gameManager.grabTurn = 0;

    }
    public void HurtAnim() {
    
    }

    public void SetPlayer(int playerNum, int characterNum, int opponentNum) {
        gameObject.name = ("Player" + playerNum);
        if (playerNum == 2) {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        this.characterNum = characterNum;
        this.opponentNum = opponentNum;
        
    }
    
}
