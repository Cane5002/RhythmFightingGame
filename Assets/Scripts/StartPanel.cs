using UnityEngine.SceneManagement;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update() {
        if (gameManager.gameEnd) {
            gameObject.GetComponent<Animator>().SetTrigger("GameEnd");
        }
    }

    public void StartGame() {
        gameManager.gameStartTrigger = true;
    }

    public void EndGame() {
        SceneManager.LoadScene(0);
    }
}
