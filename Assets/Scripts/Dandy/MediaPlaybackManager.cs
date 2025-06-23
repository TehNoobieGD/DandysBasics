using UnityEngine;

public class MediaPlaybackManager : MonoBehaviour
{
    public static MediaPlaybackManager Instance { get; private set; }

    public static bool PauseGame { get; set; } = false;

    public bool IsMediaPlaying { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartMedia()
    {
        IsMediaPlaying = true;
        PauseGame = true;
    }

    public void StopMedia()
    {
        IsMediaPlaying = false;
        PauseGame = false;
    }
}
