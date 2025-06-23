using UnityEngine;

public class QuitOnEnemyCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the parent collided with an object tagged "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with enemy! Quitting game...");
            QuitGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Also handle trigger collisions if your enemy collider is a trigger
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Triggered by enemy! Quitting game...");
            QuitGame();
        }
    }

    private void QuitGame()
    {
        // If running in the Unity editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the built application
        Application.Quit();
#endif
    }
}
