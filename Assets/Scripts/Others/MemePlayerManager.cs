using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MemePlayerManager : MonoBehaviour
{
    [Header("Canvases (assign in Inspector)")]
    public GameObject memesCanvas;         // Holds VideoPlayers (hidden)
    public GameObject memePlayerCanvas;    // Visual container for video output
    public GameObject blackCanvas;         // Background canvas (fullscreen black)
    public GameObject blackOverlayCanvas;  // Optional second overlay canvas

    private readonly List<VideoPlayer> memeVideos = new();

    void Awake()
    {
        if (memesCanvas == null)
        {
            Debug.LogError("Memes canvas is not assigned.");
            enabled = false;
            return;
        }

        memeVideos.AddRange(memesCanvas.GetComponentsInChildren<VideoPlayer>(true));

        if (memeVideos.Count == 0)
            Debug.LogWarning("No VideoPlayers found under Memes canvas.");
    }

    public void PlayRandomMeme()
    {
        if (memesCanvas == null || memePlayerCanvas == null || blackCanvas == null)
        {
            Debug.LogError("One or more canvases are not assigned!");
            return;
        }

        if (memeVideos.Count == 0)
        {
            Debug.LogWarning("No meme videos available to play.");
            return;
        }

        foreach (var vp in memeVideos) vp.Stop();

        VideoPlayer sourcePlayer = memeVideos[Random.Range(0, memeVideos.Count)];
        GameObject vpGO = sourcePlayer.gameObject;
        string memeName = vpGO.name;

        Transform displayTf = FindDeepChild(memePlayerCanvas.transform, memeName);
        if (displayTf == null)
        {
            Debug.LogWarning($"Display object '{memeName}' not found in MemePlayer canvas.");
            return;
        }

        // Activate black background + optional overlay
        blackCanvas.SetActive(true);
        if (blackOverlayCanvas != null)
            blackOverlayCanvas.SetActive(true);

        memePlayerCanvas.SetActive(true);

        foreach (Transform child in memePlayerCanvas.transform)
            child.gameObject.SetActive(child == displayTf);

        vpGO.SetActive(true);
        sourcePlayer.time = 0;

        // Pause the game when video starts
        MediaPlaybackManager.PauseGame = true;

        sourcePlayer.Play();

        StartCoroutine(DisableAfterVideoEnds(sourcePlayer, displayTf.gameObject, vpGO));
    }

    private IEnumerator DisableAfterVideoEnds(VideoPlayer player, GameObject displayObject, GameObject vpGO)
    {
        while (player.frame <= 0) yield return null;
        while (player.isPlaying) yield return null;

        player.Stop();

        displayObject.SetActive(false);
        memePlayerCanvas.SetActive(false);
        blackCanvas.SetActive(false);
        if (blackOverlayCanvas != null)
            blackOverlayCanvas.SetActive(false);
        vpGO.SetActive(false);

        // Resume the game after video ends
        MediaPlaybackManager.PauseGame = false;
    }

    private Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;

            Transform result = FindDeepChild(child, name);
            if (result != null) return result;
        }
        return null;
    }
}
