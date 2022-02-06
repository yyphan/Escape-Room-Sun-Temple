using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public Slider countdownSlider;
    public int countdownTimeInSeconds = 15;

    private bool isCounting = false;
    private float normalizedInterval;

    public void StartCountdown()
    {
        normalizedInterval = (float)1 / countdownTimeInSeconds;
        isCounting = true;
    }

    public void StopCountdown()
    {
        isCounting = false;
    }

    private void Update()
    {
        if (countdownSlider.value <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("GameOverFailure");
        }
        else if (isCounting)
        {
            countdownSlider.value -= normalizedInterval * Time.deltaTime;
        }
    }
}
