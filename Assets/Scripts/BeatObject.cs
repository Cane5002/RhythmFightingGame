using UnityEngine;
using System.Collections.Generic;

public class BeatObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode attackKey, defendKey, grabKey, dodgeKey;
    public Animator playerAnimator;
    public BeatMaker beatStarter;
    public GameObject beatHit;
    public GameObject playerObject;
    public SpriteRenderer beatSprite;
    public GameManager gameManager;
    public Color p1Color, p2Color;
    public Color[] beatColor = new Color[2];
    public int playerNum;
    
    // Start is called before the first frame update
    void Start() {
        gameManager = GameManager.FindObjectOfType<GameManager>();
        beatStarter = GameObject.FindObjectOfType<BeatMaker>();
    }

    // Update is called once per frame
    void Update() {

        if (canBePressed && !gameManager.gameEnd) {
            //------------------------------Action input------------------------------
            if (Input.GetKeyDown(attackKey)) { //ATTACK
                canBePressed = false;
                DeactivateBeat();
                gameManager.PlayerChoice(playerNum, "Attack");
                //attackPriority
                /*if (Mathf.Abs(transform.position.x - beatHit.transform.position.x) > 0.25) {
                    GameManager.instance.attackPriority[playerNum - 1] = 1;
                    Debug.Log("Hit");
                }
                else if (Mathf.Abs(transform.position.x - beatHit.transform.position.x) > 0.05) {
                    GameManager.instance.attackPriority[playerNum - 1] = 2;
                    Debug.Log("Good");
                }
                else {
                    GameManager.instance.attackPriority[playerNum - 1] = 3;
                    Debug.Log("Perfect!!!");
                }*/
                Debug.Log("ATTACK!!!");
                playerObject.SendMessage("AttackPrep", priorityEval());
            }

            if (Input.GetKeyDown(defendKey)) { //DEFEND
                canBePressed = false;
                DeactivateBeat();
                gameManager.PlayerChoice(playerNum, "Defend");
                playerObject.SendMessage("DefendPrep", priorityEval());
                
            }

            if (Input.GetKeyDown(grabKey)) { //GRAB
                canBePressed = false;
                DeactivateBeat();
                gameManager.PlayerChoice(playerNum, "Grab");
                playerObject.SendMessage("GrabPrep", priorityEval());
            }

            if (Input.GetKeyDown(dodgeKey)) { //DODGE
                canBePressed = false;
                DeactivateBeat();
                gameManager.PlayerChoice(playerNum, "Dodge");
                playerObject.SendMessage("DodgePrep", priorityEval());
            }
        }
    }

    public void beatSet(int playerNum) {
        //Debug.Log("PlayerNum: " + playerNum);
        beatHit = GameObject.Find("p" + playerNum + "BeatHit");
        playerObject = GameObject.Find("Player" + playerNum);
        playerAnimator = playerObject.GetComponent<Animator>();
        //beatSprite.color = beatColor[playerNum - 1];
        if (playerNum == 1) {
            beatSprite.color = p1Color;
            attackKey = GameManager.instance.p1attackKey;
            defendKey = GameManager.instance.p1defendKey;
            grabKey = GameManager.instance.p1grabKey;
            dodgeKey = GameManager.instance.p1dodgeKey;
        }
        else {
            beatSprite.color = p2Color;
            attackKey = GameManager.instance.p2attackKey;
            defendKey = GameManager.instance.p2defendKey;
            grabKey = GameManager.instance.p2grabKey;
            dodgeKey = GameManager.instance.p2dodgeKey;
        }
        this.playerNum = playerNum;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Activator") {
            canBePressed = true;
        }

        if (other.tag == "DeActivator") {
            gameManager.PlayerChoice(playerNum, "Idle");
            playerObject.SendMessage("IdlePrep", priorityEval());
            DeactivateBeat();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Activator") {
            canBePressed = false;
        }
    }

    public void DeactivateBeat() {
        if (playerAnimator.GetBool("Grapple")) {
            if (gameManager.grabTurn == 0) {
                gameManager.EndGrab();
            }
            --gameManager.grabTurn;
        }
        //--beatStarter.pBeatsSize[playerNum - 1];
        beatStarter.pBeat[playerNum - 1].RemoveAt(0);
        Destroy(gameObject, 0);
    }

    public int priorityEval() {
        int priority = 0;
        if (Mathf.Abs(transform.position.x - beatHit.transform.position.x) > 0.25) {
            priority = 1;
            //Debug.Log("Hit");
        }
        else if (Mathf.Abs(transform.position.x - beatHit.transform.position.x) > 0.05) {
            priority = 2;
            //Debug.Log("Good");
        }
        else {
            priority = 3;
            //Debug.Log("Perfect!!!");
        }
        return priority;
    }

    
}

