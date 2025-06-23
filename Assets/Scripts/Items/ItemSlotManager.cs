using UnityEngine;
using UnityEngine.UI;

public class ItemFrom3DClick : MonoBehaviour
{
    [Header("Assign the UI RawImage prefab to clone")]
    public RawImage itemUIPrefab;

    [Header("Slot targets (UI Image positions)")]
    public RectTransform[] slots;

    [Header("Assign the 3D clickable GameObject (like a cube)")]
    public GameObject clickable3DObject;

    private bool itemCollected = false;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !itemCollected)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == clickable3DObject)
                {
                    CollectItemToSlot();
                }
            }
        }
    }

    void CollectItemToSlot()
    {
        if (itemUIPrefab == null || slots.Length == 0)
        {
            Debug.LogWarning("Item prefab or slots not assigned.");
            return;
        }

        // Clone the UI item
        RawImage clonedItem = Instantiate(itemUIPrefab, itemUIPrefab.transform.parent);
        clonedItem.gameObject.SetActive(true);             // Ensure it's visible
        clonedItem.color = Color.white;                    // Make sure alpha is 1

        // Place it in Slot 1
        RectTransform clonedRect = clonedItem.GetComponent<RectTransform>();
        clonedRect.position = slots[0].position;
        clonedRect.SetAsLastSibling();                     // Optional: bring to front

        itemCollected = true;
    }
}
