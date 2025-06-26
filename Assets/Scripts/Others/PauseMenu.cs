using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    [Header("Drag your pause menu Canvas here")]
    public GameObject pauseCanvas;

    public bool isPaused = false;

    void Start()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Ne pause pas si une vidéo est en cours
            if (IsVideoPlaying())
                return;

            if(Collector.settings.gameObject.activeSelf)
            {
                return;
            }

            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    bool IsVideoPlaying()
    {
        VideoPlayer[] allVideos = GameObject.FindObjectsOfType<VideoPlayer>();
        foreach (VideoPlayer vp in allVideos)
        {
            if (vp.isPlaying)
                return true;
        }
        return false;
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PauseManager.PauseGame = true;
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        isPaused = false;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PauseManager.PauseGame = false;
        Time.timeScale = 1f;
    }
}
