using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class NotebookCounter : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI notebookText;

    [Header("Objects to Clear at 2 Notebooks")]
    public GameObject objectToClear;
    public GameObject secondObjectToClear;  // New drag and drop box for second object to clear

    [Header("End Sequence")]
    public AudioClip finalClip;                    // Drag your audio clip here
    public AudioSource audioSource;                // Drag an AudioSource here

    private int totalNotebooks;
    private int collectedNotebooks = 0;
    private bool finalSequenceStarted = false;

    private List<GameObject> books = new List<GameObject>();

    void Start()
    {
        RefreshNotebookList();
        UpdateNotebookText();
    }

    void Update()
    {
        int currentCount = CountRemainingBooks();

        if (currentCount != (totalNotebooks - collectedNotebooks))
        {
            collectedNotebooks = totalNotebooks - currentCount;
            UpdateNotebookText();

            if (collectedNotebooks == 2)
            {
                if (objectToClear != null)
                {
                    foreach (Transform child in objectToClear.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                if (secondObjectToClear != null)
                {
                    foreach (Transform child in secondObjectToClear.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

            if (collectedNotebooks == totalNotebooks && !finalSequenceStarted)
            {
                finalSequenceStarted = true;
                StartCoroutine(PlayFinalClipAfterMedia());
            }
        }
    }

    void RefreshNotebookList()
    {
        GameObject[] foundBooks = GameObject.FindGameObjectsWithTag("Book");
        books = new List<GameObject>(foundBooks);
        totalNotebooks = books.Count;
    }

    int CountRemainingBooks()
    {
        int remaining = 0;
        foreach (GameObject book in books)
        {
            if (book != null)
                remaining++;
        }
        return remaining;
    }

    void UpdateNotebookText()
    {
        if (notebookText != null)
        {
            notebookText.text = $"Notebooks : {collectedNotebooks}/{totalNotebooks}";
        }
    }

    IEnumerator PlayFinalClipAfterMedia()
    {
        // Wait a short moment to let any final media (like video/audio from notebook) start playing
        yield return new WaitForSeconds(0.1f);
        yield return null; // wait 1 more frame

        // Wait until all VideoPlayers and AudioSources stop playing
        while (IsAnyVideoPlaying() || IsAnyAudioPlaying())
        {
            yield return null; // wait a frame
        }

        // Play final audio
        if (audioSource != null && finalClip != null)
        {
            audioSource.PlayOneShot(finalClip);
        }
    }

    bool IsAnyVideoPlaying()
    {
        VideoPlayer[] videos = FindObjectsOfType<VideoPlayer>();
        foreach (VideoPlayer vp in videos)
        {
            if (vp.isPlaying)
                return true;
        }
        return false;
    }

    bool IsAnyAudioPlaying()
    {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in audios)
        {
            if (source != null && source.isPlaying)
                return true;
        }
        return false;
    }
}
