using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour
{
    [Tooltip("The canvas to show when the game is won")]
    public GameObject winCanvas;

    private bool hasWon = false;

    private void Start()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasWon)
        {
            hasWon = true;
            StartCoroutine(TriggerWin());
        }
    }

    IEnumerator TriggerWin()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true);

        // Forcefully unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Wait one frame before pausing (ensures cursor is unlocked)
        yield return null;

        Time.timeScale = 0f;
        Debug.Log("You Win!");
    }
}
