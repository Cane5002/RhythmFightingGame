using UnityEngine;

public class BeatScroll : MonoBehaviour
{
    public float beatTempo;
    public int playerNum;
    public BeatObject beat;
    
    // Start is called before the first frame update
    void Start() {
        beatTempo = GameManager.instance.beatTempo / 60f;
        playerNum = beat.playerNum;
    }

    // Update is called once per frame
    void Update() {
        if (playerNum == 1) {
            transform.position -= new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        } else if (playerNum == 2) {
            transform.position += new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        }
    }
}
