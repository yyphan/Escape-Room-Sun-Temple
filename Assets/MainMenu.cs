using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // click play button to enter game
    public void PlayGame()
    {
        SceneManager.LoadScene("EscapeRoom");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Entry");
    }
}
