using UnityEngine;

public class WelcomeMap : MonoBehaviour
{
    [Tooltip("Assign an AudioSource that plays the welcome audio")]
    public AudioSource welcomeAudioSource;

    private bool hasPlayed = false;

    private void Start()
    {
        if (welcomeAudioSource != null && !hasPlayed)
        {
            welcomeAudioSource.Play();
            hasPlayed = true;
        }
        else if (welcomeAudioSource == null)
        {
            Debug.LogWarning("WelcomeMap: AudioSource is not assigned.");
        }
    }
}
