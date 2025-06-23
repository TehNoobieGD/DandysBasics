using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MemeCloseButton : MonoBehaviour
{
    [Header("References (assign in Inspector)")]
    public GameObject memesCanvas;         // holds the VideoPlayers
    public GameObject memePlayerCanvas;    // UI that shows the meme
    public GameObject blackCanvas;         // black background
    public GameObject blackOverlay;        // optional extra overlay
    public Button     closeButton;         // the TMP / UI Button

    void Start ()
    {
        if (closeButton == null)
        {
            Debug.LogError("Close Button reference not assigned!");
            enabled = false;
            return;
        }

        closeButton.onClick.AddListener(OnCloseClicked);
    }

    /* ------------------------------------------------------------ */
    private void OnCloseClicked ()
    {
        Debug.Log("Close button clicked. Stopping and hiding everything.");

        /* 1️⃣  Stop every VideoPlayer under memePlayerCanvas
               (these are only the visible “screens”)                */
        foreach (Transform child in memePlayerCanvas.transform)
        {
            VideoPlayer vp = child.GetComponent<VideoPlayer>();
            if (vp != null && vp.isPlaying)
            {
                vp.Stop();      // halts both video & audio
                vp.time = 0;    // rewind to the first frame
            }

            child.gameObject.SetActive(false);   // hide the visual
        }

        /* 2️⃣  ALSO stop the hidden source players in memesCanvas,
               in case one is still buffering / playing audio        */
        foreach (VideoPlayer vp in memesCanvas.GetComponentsInChildren<VideoPlayer>(true))
        {
            if (vp.isPlaying)
            {
                vp.Stop();
                vp.time = 0;
            }
        }

        /* 3️⃣  Hide the canvases (visuals only – players stay active
               in the hierarchy so they can be reused)               */
        memePlayerCanvas.SetActive(false);
        blackCanvas     .SetActive(false);
        if (blackOverlay != null) blackOverlay.SetActive(false);
    }
}
