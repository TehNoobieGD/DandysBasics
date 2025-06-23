using UnityEngine;

public class ExitCheck : MonoBehaviour
{
    [Tooltip("Parent GameObject that contains cubes or other objects to enable when all books are collected")]
    public GameObject groupToEnable;

    private void Update()
    {
        // Check if all "Book" objects have been destroyed
        GameObject[] books = GameObject.FindGameObjectsWithTag("Book");

        // If none remain and the group is still disabled
        if (books.Length == 0 && groupToEnable != null && !groupToEnable.activeSelf)
        {
            groupToEnable.SetActive(true);
            Debug.Log("All books collected! Exit group enabled.");
        }
    }
}
