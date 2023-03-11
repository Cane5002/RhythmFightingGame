using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CombatManager combatManager;
    public BeatMaker beatStarter;
    public Animator p1Animator, p2Animator; 
    public GameObject beatHit, p1, p2;
    public KeyCode startKey;
    public KeyCode p1attackKey, p1defendKey, p1grabKey, p1dodgeKey;
    public KeyCode p2attackKey, p2defendKey, p2grabKey, p2dodgeKey;
    public HealthBar p1HealthBar, p2HealthBar;
    public StartPanel transitionPanel;
    public AudioSource musicTrack;
    public bool gameStart = false, gameStartTrigger = false, canBePressed, grabState, evalComp=true, gameEnd = false;
    public int grabTurn;
    public string outcomeEval;
    public float beatTempo, timeSince;
    public string[] playerChoice = new string[2];
    public int[] playerState = new int[2], attackPriority = new int[2], playerMaxHealth = new int[] {100,100}, playerHealth = new int[] { 100, 100 };
    public GameObject[] characterPrefabs;
    public Transform p1SpawnPoint, p2SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        int p1CharSelect = PlayerPrefs.GetInt("p1CharSelect");
        int p2CharSelect = PlayerPrefs.GetInt("p2CharSelect");
        p1 = Instantiate(characterPrefabs[p1CharSelect], p1SpawnPoint.position, Quaternion.identity);
        p1.GetComponent<Character>().SetCharacter(1, 2, p2CharSelect);
        p2 = Instantiate(characterPrefabs[p2CharSelect], p2SpawnPoint.position, Quaternion.identity);
        p2.GetComponent<Character>().SetCharacter(2, 1, p1CharSelect);
        p1Animator = p1.GetComponent<Animator>();
        p1Animator.SetInteger("Character", p2CharSelect);
        p2Animator = p2.GetComponent<Animator>();
        p2Animator.SetInteger("Character", p1CharSelect);
        playerMaxHealth = new int[] {p1.GetComponent<Character>().maxHealth, p2.GetComponent<Character>().maxHealth};
        playerHealth = new int[] {playerMaxHealth[0], playerMaxHealth[1]};
        evalComp = true;

        //attackPriority[1] = 1;
        //playerChoice[1] = "Defend";


    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!gameStart) {
            if (gameStartTrigger) {
                beatStarter.hasStarted = true;
                gameStart = true;
                musicTrack.Play();
            }
        } else if (gameEnd) {
            if (playerHealth[0] > playerHealth[1]) {
                p1Animator.SetTrigger("Victory");
                p2Animator.SetTrigger("Defeat");

            } else {
                p2Animator.SetTrigger("Victory");
                p1Animator.SetTrigger("Defeat");
            }
            transitionPanel.GetComponent<Animator>().SetTrigger("GameEnd");
        } else if (evalComp)/*if (!(playerChoice[0].Equals("Nothing")) && !(playerChoice[1].Equals("Nothing")))*/ {
            /*if (grabState) {
                outcomeEval = grappleEval(playerChoice, attackPriority, playerState);
            }
            else {
                outcomeEval = roundEval(playerChoice, attackPriority, playerState);
            }
            playerChoice[0] = "Nothing";
            playerChoice[1] = "Nothing";
            Invoke(outcomeEval, 0f);*/
            combatManager.TryCombat();
            
        }
    }

    public static string roundEval (string[] playerChoice, int[] AttackPriority, int[] playerState) {
        string outcome = "nothing";
        if (playerChoice[0].Equals("Attack")) {
            if (playerChoice[1].Equals("Attack")) {
                if (AttackPriority[0] > AttackPriority[1]) {
                    playerState[0] = 1;
                    playerState[1] = 0;
                } else if (AttackPriority[0] < AttackPriority[1]) {
                    playerState[1] = 1;
                    playerState[0] = 0;
                } else {
                    playerState[1] = playerState[0];
                }
                outcome = "AvA";
            } else if (playerChoice[1].Equals("Defend")) {
                playerState[1] = 1;
                playerState[0] = 0;
                outcome = "AvD";
            } else if (playerChoice[1].Equals("Idle")) {
                playerState[0] = 1;
                playerState[1] = 0;
                outcome = "AvI";
            } else if (playerChoice[1].Equals("Grab")) {
                playerState[0] = 1;
                playerState[1] = 0;
                outcome = "AvI";
            } else if (playerChoice[1].Equals("Dodge")) {
                playerState[1] = 1;
                playerState[0] = 0;
                outcome = "nothing";
            }
        } else if (playerChoice[0].Equals("Defend")) {
            if (playerChoice[1].Equals("Attack")) {
                playerState[0] = 1;
                playerState[1] = 0;
                outcome = "AvD";
            } else if (playerChoice[1].Equals("Defend")) {
                outcome = "nothing";
            } else if (playerChoice[1].Equals("Idle")) {
                outcome = "nothing";
            } else if (playerChoice[1].Equals("Grab")) {
                playerState[0] = 0;
                playerState[1] = 1;
                outcome = "GvD";
            } else if (playerChoice[1].Equals("Dodge")) {
                playerState[0] = 1;
                playerState[1] = 0;
                outcome = "DvD";
            }
        } else if (playerChoice[0].Equals("Idle")) {
            if (playerChoice[1].Equals("Attack")) {
                playerState[1] = 1;
                playerState[0] = 0;
                outcome = "AvI";
            } else if (playerChoice[1].Equals("Defend")) {
                outcome = "nothing";
            } else if (playerChoice[1].Equals("Idle")) {
                outcome = "nothing";
            } else if (playerChoice[1].Equals("Grab")) {
                playerState[1] = 1;
                playerState[0] = 0;
                outcome = "GvD";
            } else if (playerChoice[1].Equals("Dodge")) {
                playerState[1] = 1;
                playerState[0] = 0;
                outcome = "nothing";
            }
        } else if (playerChoice[0].Equals("Grab")) {
            if (playerChoice[1].Equals("Attack")) {
                playerState[0] = 0;
                playerState[1] = 1;
                outcome = "AvI";
            }
            else if (playerChoice[1].Equals("Defend")) {
                playerState[1] = 0;
                playerState[0] = 1;
                outcome = "GvD";
            }
            else if (playerChoice[1].Equals("Idle")) {
                playerState[1] = 0;
                playerState[0] = 1;
                outcome = "GvD";
            } else if (playerChoice[1].Equals("Grab")) {
                playerState[0] = playerState[1];
                outcome = "nothing";
            } else if (playerChoice[1].Equals("Dodge")) {
                playerState[1] = 0;
                playerState[0] = 1;
                outcome = "nothing";
            }
        } else if (playerChoice[0].Equals("Dodge")) {
            if (playerChoice[1].Equals("Attack")) {
                playerState[0] = 1;
                playerState[1] = 0;
                outcome = "nothing";
            }
            else if (playerChoice[1].Equals("Defend")) {
                playerState[1] = 1;
                playerState[0] = 0;
                outcome = "DvD";
            }
            else if (playerChoice[1].Equals("Idle")) {
                playerState[1] = 0;
                playerState[0] = 1;
                outcome = "nothing";
            }
            else if (playerChoice[1].Equals("Grab")) {
                playerState[0] = 1; 
                playerState[1] = 0;
                outcome = "nothing";
            }
            else if (playerChoice[1].Equals("Dodge")) {
                playerState[1] = playerState[0];
                outcome = "nothing";
            }
        }
        return outcome;
    }

    public static string grappleEval(string[] playerChoice, int[] AttackPriority, int[] playerState) {
        string outcome = "nothing";
        int topPlayer;
        if (playerState[0] > playerState[1]) {
            topPlayer = 0;
        } else {
            topPlayer = 1;
        }
        if (playerChoice[topPlayer].Equals("Attack")) {
            outcome = "AvI";
        }
        else if (playerChoice[topPlayer].Equals("Defend")) {
            outcome = "nothing";
        }
        else if (playerChoice[topPlayer].Equals("Idle")) {
            if (playerChoice[1].Equals("Attack")) {
                playerState[1] = 1;
                playerState[0] = 0;
                outcome = "AvI";
            }
            else if (playerChoice[1-topPlayer].Equals("Defend")) {
                outcome = "nothing";
            }
            else if (playerChoice[1-topPlayer].Equals("Idle")) {
                outcome = "nothing";
            }
            else if (playerChoice[1-topPlayer].Equals("Grab")) {
                playerState[1] = 1;
                playerState[0] = 0;
                instance.EndGrab();
                outcome = "GvD";
            }
        }
        else if (playerChoice[topPlayer].Equals("Grab")) {
            outcome = "nothing";
        }
        return outcome;
    }

    /*public void AvA () {
        if (playerState[0] > playerState[1]) {
            Damage(p2HealthBar, 2, Mathf.Abs(attackPriority[0] - attackPriority[1]));
        } else if (playerState[1] > playerState[0]) {
            Damage(p1HealthBar, 1, Mathf.Abs(attackPriority[0] - attackPriority[1]));
        } else {

        }
    }

    public void AvD () {
        if (playerState[0] > playerState[1]) {
            Stun(2, 1);
        }
        else if (playerState[1] > playerState[0]) {
            Stun(1, 1);
        }
        else {
        }
    }

    public void AvI () {
        if (playerState[0] > playerState[1]) {
            Damage(p2HealthBar,2, 3);
        }
        else if (playerState[1] > playerState[0]) {
            Damage(p1HealthBar,1, 3);
        }
        else {
        }
    }

    public void GvD () {
        grabState = true;
        if (playerState[0] > playerState[1]) {
            p1.GetComponent<Character>().GrappleAnim();
            Stun(2, 1);
        }
        else if (playerState[1] > playerState[0]) {
            p2.GetComponent<Character>().GrappleAnim();
            Stun(1, 1);
        }
        else {
        }
    }

    public void nothing () {
        grabState = false;
        if (playerState[0] > playerState[1]) {
            p1Animator.SetBool("Grapple", false);
        }
        else if (playerState[1] > playerState[0]) {
            p2Animator.SetBool("Grapple", false);
        }
        ShowPlayers();
    }

    public void DvD () {
        if (playerState[0] > playerState[1]) {
            Counter(1);
        }
        else if (playerState[1] > playerState[0]) {
            Counter(2);
        }
        else {
        }
    }*/

    public void Damage (int targetPlayerNum, int damage) {
        /*int damage = 5;
        if (grabState)
            damage = 10;
        else if (attackStr == 1)
            damage = 5;
        else if (attackStr == 2)
            damage = 10;
        else if (attackStr == 3)
            damage = 15;
        */
        Debug.Log("damage: " + damage);
        playerHealth[targetPlayerNum - 1] -= damage;
        GameObject.Find("p" + targetPlayerNum + "HealthBar").GetComponent<HealthBar>().SetHealth(playerHealth[targetPlayerNum - 1]);
        if (playerHealth[targetPlayerNum - 1] > playerMaxHealth[targetPlayerNum - 1]) {
            playerHealth[targetPlayerNum -1] = playerMaxHealth[targetPlayerNum - 1];
        }
        if (targetPlayerNum == 1) {
            p1Animator.SetTrigger("Hurt");
        }
        else if (targetPlayerNum == 2) {
            p2Animator.SetTrigger("Hurt");
        }
    }

    public void Stun(int targetPlayerNum, int duration) {
        //GameObject beat = beatStarter.pBeats[playerNum - 1, beatStarter.pBeatsSize[playerNum - 1] - 1];
        GameObject beat;
        for (int i = 0; i < duration; i++) {
            beat = beatStarter.pBeat[targetPlayerNum - 1][i];
            beat.GetComponent<BeatObject>().DeactivateBeat();
        }
        playerChoice[targetPlayerNum - 1] = "Idle";
        combatManager.stun = duration;
        if (targetPlayerNum == 2)
            p2Animator.SetTrigger("Stun");
        else
            p1Animator.SetTrigger("Stun");
    }

    public void Counter(int targetPlayerNum) {
        beatStarter.subBeat(targetPlayerNum);
        playerChoice[playerChoice.Length - targetPlayerNum] = "Idle";
        combatManager.stun = 1;
    }

    public void PlayerChoice (int playerNum, string choiceName) {
        playerChoice[playerNum - 1] = choiceName;
    }

    public void HidePlayer (int playerNum) {
        if (playerNum == 2) {
            p2.GetComponent<SpriteRenderer>().enabled = false;
        } else {
            p1.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ShowPlayers () {
        if (!p2.GetComponent<SpriteRenderer>().enabled) {
            p2.GetComponent<SpriteRenderer>().enabled = true;
        } else if (!p1.GetComponent<SpriteRenderer>().enabled) {
            p1.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    public void EndGrab() {
        grabState = false;
        p1Animator.SetBool("Grapple", false);
        p2Animator.SetBool("Grapple", false);
        ShowPlayers();
        grabTurn = 0;
    }
}
