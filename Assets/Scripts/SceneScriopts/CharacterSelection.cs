using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characterImages;
    public int selectedCharacter = 0;

    public void NextCharacter() {
        characterImages[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characterImages.Length;
        characterImages[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter() {
        characterImages[selectedCharacter].SetActive(false);
        if (selectedCharacter == 0) {
            selectedCharacter = characterImages.Length - 1;
        } else {
            selectedCharacter -= 1;
        }
        characterImages[selectedCharacter].SetActive(true);
    }
}
