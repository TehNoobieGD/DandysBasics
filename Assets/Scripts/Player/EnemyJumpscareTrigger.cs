using UnityEngine;
using UnityEngine.AI;

public class EnemyJumpscareTrigger : MonoBehaviour
{
    public GameObject player;
    public Camera mainCamera;
    public PlayerMovement playerMovementScript;  // Your full PlayerMovement component
    public NavMeshAgent enemyAgent;               // Optional, assign if enemy uses NavMeshAgent
    public AudioSource jumpscareAudioSource;     // Assign an AudioSource with jumpscare clip
    public AudioClip jumpscareClip;

    private bool hasJumpscared = false;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Lock cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasJumpscared)
            return;

        if (other.gameObject == player)
        {
            TriggerJumpscare();
        }
    }

    private void TriggerJumpscare()
    {
        hasJumpscared = true;

        // Disable the player's movement script to freeze input
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // Stop enemy NavMeshAgent if any
        if (enemyAgent != null)
            enemyAgent.isStopped = true;

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Play jumpscare sound
        if (jumpscareAudioSource != null && jumpscareClip != null)
        {
            jumpscareAudioSource.clip = jumpscareClip;
            jumpscareAudioSource.Play();
        }

        // Start camera shake coroutine
        if (mainCamera != null)
            StartCoroutine(CameraShake(0.5f, 0.4f));
    }

    private System.Collections.IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }
}
