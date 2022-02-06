using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlameTrigger : MonoBehaviour
{
    public List<ParticleSystem> flameParticles;
    public string textToDisplay;
    public UnityEvent<string> onEnterChangeText;

    private void OnTriggerEnter(Collider other)
    {
        // check if the other collider is player
        if (other.gameObject.CompareTag("Player"))
        {
            // set the particle systems to play
            foreach(ParticleSystem ps in flameParticles)
            {
                ps.Play();
            }

            onEnterChangeText.Invoke(textToDisplay);
        }
    }
}
