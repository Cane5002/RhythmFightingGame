using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMaker : MonoBehaviour
{
    public bool hasStarted, isUIBeat;
    public float beatTempo, time, beatRate;
    public GameObject beatPrefab, p1Beat, p2Beat;
    public GameManager gameManager;
    public GameObject[,] pBeats = new GameObject[2, 8];
    public int[] pBeatsSize = new int[2];
    public List<GameObject>[] pBeat = new List<GameObject>[2];

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.FindObjectOfType<GameManager>();
        beatTempo = gameManager.beatTempo / 60f;
        beatRate = 2 / beatTempo;
        pBeat[0] = new List<GameObject>();
        pBeat[1] = new List<GameObject>();
        
        /*while (!hasStarted) {
        }
        InvokeRepeating("MakeBeat", 0f, 1 / beatTempo);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && !gameManager.gameEnd) {
            time += Time.deltaTime;
            if (time >= beatRate) {
                time = time % 1f;
                /*for (int i = pBeats.GetLength(0); i > 0; --i) {
                    pBeats[0, i] = pBeats[0, i - 1];
                    pBeats[1, i] = pBeats[1, i - 1];
                }*/
                p1Beat = MakeBeat(1);
                pBeat[0].Add(p1Beat);
                //pBeats[0,0] = p1Beat;
                //++pBeatsSize[0];
                p2Beat = MakeBeat(2);
                pBeat[1].Add(p2Beat);
                //pBeats[1,0] = (p2Beat);
                //++pBeatsSize[1];
            }
            
        }
    }

    public GameObject MakeBeat (int playerNum) {
        GameObject beat;
        BeatObject playerBeat;
        beat = Instantiate(beatPrefab, transform.position, transform.rotation);
        playerBeat = beat.GetComponent<BeatObject>();
        playerBeat.beatSet(playerNum);
        return beat;
    }

    public GameObject subBeat (int playerNum) {
        GameObject beat, beatHit;
        BeatObject playerBeat;
        beatHit = GameObject.Find("p" + playerNum + "BeatHit");
        if (playerNum == 1) {
            beat = Instantiate(beatPrefab, beatHit.transform.position + new Vector3(beatRate, 0), transform.rotation);
        } else {
            beat = Instantiate(beatPrefab, beatHit.transform.position - new Vector3(beatRate, 0), transform.rotation);
        }
        playerBeat = beat.GetComponent<BeatObject>();
        playerBeat.beatSet(playerNum);
        pBeat[playerNum - 1].Insert(0, beat);
        Debug.Log("COUNTER");
        return beat;
    }
}
