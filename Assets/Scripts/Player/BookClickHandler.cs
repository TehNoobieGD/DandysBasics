using UnityEngine;
using UnityEngine.UI;

public class BookClickHandler : MonoBehaviour
{
    [Header("Tag to detect")]
    public string targetTag = "Book";

    [Header("Max distance to collect book")]
    public float maxCollectDistance = 3f;

    [Header("Cursor UI Images")]
    public Image normalCursorUI;
    public Image clickCursorUI;

    private bool isLookingAtBook = false;

    void Start()
    {
        ShowNormalCursor();
    }

    void Update()
    {
        UpdateCursorUI();

        if (Input.GetMouseButtonDown(0) && isLookingAtBook)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag(targetTag))
                {
                    float distance = Vector3.Distance(Collector.playerObj.transform.position, hit.collider.transform.position);
                    if (distance <= maxCollectDistance)
                    {
                        Destroy(hit.collider.gameObject);
                        Debug.Log("Destroyed object with tag: " + targetTag);

                        MemePlayerManager memePlayer = FindObjectOfType<MemePlayerManager>();
                        if (memePlayer != null)
                            memePlayer.PlayRandomMeme();
                        else
                            Debug.LogWarning("MemePlayerManager not found in scene.");
                    }
                    else
                    {
                        Debug.Log("Too far to collect the book!");
                    }
                }
            }
        }
    }

    void UpdateCursorUI()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // center screen ray
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                float distance = Vector3.Distance(Collector.playerObj.transform.position, hit.collider.transform.position);
                if (distance <= maxCollectDistance)
                {
                    if (!isLookingAtBook)
                    {
                        ShowClickCursor();
                        isLookingAtBook = true;
                    }
                    return;
                }
            }
        }

        if (isLookingAtBook)
        {
            ShowNormalCursor();
            isLookingAtBook = false;
        }
    }

    void ShowNormalCursor()
    {
        if (normalCursorUI != null) normalCursorUI.enabled = true;
        if (clickCursorUI != null) clickCursorUI.enabled = false;
    }

    void ShowClickCursor()
    {
        if (normalCursorUI != null) normalCursorUI.enabled = false;
        if (clickCursorUI != null) clickCursorUI.enabled = true;
    }
}
