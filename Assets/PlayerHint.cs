using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHint : MonoBehaviour
{
    public TextMeshProUGUI playerHintText;

    private string gameStartHint;

    // Awake is called before Start()
    private void Awake()
    {
        gameStartHint = "According to my sources, the Artifact is just ahead." +
            "Time to take it back";
    }

    // Start is called before the first frame update
    void Start()
    {
        // update hint
        SetPlayerHintText(gameStartHint);
    }

    IEnumerator PlayerHintFade(string hintText)
    {
        // displat text
        playerHintText.text = hintText;

        // wait for some time
        yield return new WaitForSeconds(5);

        // set text to empty
        playerHintText.text = "";
    }

    public void SetPlayerHintText(string playerHint)
    {
        StartCoroutine(PlayerHintFade(playerHint));
    }

}
