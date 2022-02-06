using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeTrigger : MonoBehaviour
{
    public SunTemple.CharController_Motor player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GameOverSuccess());
        }
    }

    private IEnumerator GameOverSuccess()
    {
        player.SetPlayerHint("Ah, fresh air!");
        player.StopCountdown();

        yield return new WaitForSeconds(3);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOverSuccess");
    }
}
