﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void CharacterSelect() {

    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit");
    }
}
