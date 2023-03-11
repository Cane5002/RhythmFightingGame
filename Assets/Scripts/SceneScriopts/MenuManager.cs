using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu, OptionsMenu, CharacterSelectMenu;
    public void PlayGame() {
        PlayerPrefs.SetInt("p1CharSelect", GameObject.Find("P1CharSelect").GetComponent<CharacterSelection>().selectedCharacter);
        PlayerPrefs.SetInt("p2CharSelect", GameObject.Find("P2CharSelect").GetComponent<CharacterSelection>().selectedCharacter);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Start() {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        CharacterSelectMenu.SetActive(false);
    }
}
