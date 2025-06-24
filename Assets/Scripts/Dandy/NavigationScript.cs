using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaldiAI : MonoBehaviour
{
    public Transform player;
    public AudioClip clapClip;
    public AudioClip jumpscareClip;
    public Camera playerCamera;
    public Animator myAnim;

    public float dashDistance = 3f;
    public float dashSpeed = 20f;
    public float minWaitTime = 0.8f;
    public float maxWaitTime = 2.0f;

    public bool isPaused = false;

    private NavMeshAgent agent;
    private AudioSource audioSrc;
    private bool isJumpscareTriggered = false;
    private int totalBooks;
    private bool aiStarted = false;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSrc = GetComponent<AudioSource>();
        totalBooks = GameObject.FindGameObjectsWithTag("Book").Length;

        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Update()
    {
        // Arrête l'IA si le jeu est en pause, une vidéo/media joue,
        // si le jumpscare est déclenché, ou si le curseur est visible & déverrouillé
        if (PauseManager.PauseGame || isPaused || isJumpscareTriggered ||
            (MediaPlaybackManager.Instance != null && MediaPlaybackManager.Instance.IsMediaPlaying) ||
            (Cursor.visible && Cursor.lockState == CursorLockMode.None))
        {
            agent.isStopped = true;

            if (isPaused || (Cursor.visible && Cursor.lockState == CursorLockMode.None))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            return;
        }
        else
        {
            agent.isStopped = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (!aiStarted)
        {
            int currentBooks = GameObject.FindGameObjectsWithTag("Book").Length;
            int destroyed = totalBooks - currentBooks;

            if (destroyed >= 2)
            {
                aiStarted = true;
                StartCoroutine(DashLoop());
            }
        }
    }

    IEnumerator DashLoop()
    {
        while (!isJumpscareTriggered)
        {
            // Attend tant que jeu en pause, IA en pause, média en lecture ou curseur visible/déverrouillé
            while (PauseManager.PauseGame || isPaused ||
                   (MediaPlaybackManager.Instance != null && MediaPlaybackManager.Instance.IsMediaPlaying) ||
                   (Cursor.visible && Cursor.lockState == CursorLockMode.None))
            {
                yield return null;
            }

            if (player != null)
            {
                agent.SetDestination(player.position);

                yield return null;

                if (agent.path.status != NavMeshPathStatus.PathComplete)
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                if (agent.path.corners.Length >= 2)
                {
                    Vector3 nextCorner = agent.path.corners[1];
                    Vector3 dashDir = (nextCorner - transform.position).normalized;
                    Vector3 flatDashDir = new Vector3(dashDir.x, 0f, dashDir.z);

                    if (Physics.Raycast(transform.position + Vector3.up * 0.5f, flatDashDir, out RaycastHit hit, dashDistance))
                    {
                        if (hit.collider.CompareTag("Wall"))
                        {
                            PlayClap();
                            yield return StartCoroutine(MoveToCorner(nextCorner));
                        }
                        else
                        {
                            Vector3 dashTarget = transform.position + flatDashDir * dashDistance;
                            PlayClap();
                            yield return StartCoroutine(DashTo(dashTarget));
                        }
                    }
                    else
                    {
                        Vector3 dashTarget = transform.position + flatDashDir * dashDistance;
                        PlayClap();
                        yield return StartCoroutine(DashTo(dashTarget));
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }

            float wait = GetCurrentWaitTime();
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator MoveToCorner(Vector3 corner)
    {
        agent.isStopped = false;
        agent.SetDestination(corner);

        while (Vector3.Distance(transform.position, corner) > 0.2f && !isJumpscareTriggered)
        {
            if (PauseManager.PauseGame || isPaused)
            {
                agent.isStopped = true;
                yield return null;
                continue;
            }

            agent.isStopped = false;
            yield return null;
        }

        agent.isStopped = true;
    }

    IEnumerator DashTo(Vector3 dashTarget)
    {
        Vector3 start = transform.position;
        Vector3 end = new Vector3(dashTarget.x, start.y, dashTarget.z);

        float travelTime = dashDistance / dashSpeed;
        float elapsed = 0f;

        Vector3 lookDir = (end - start).normalized;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);

        while (elapsed < travelTime && !isJumpscareTriggered)
        {
            if (PauseManager.PauseGame || isPaused)
            {
                yield return null;
                continue;
            }

            float step = dashSpeed * Time.deltaTime;
            Vector3 nextPos = Vector3.MoveTowards(transform.position, end, step);

            agent.Move(nextPos - transform.position);

            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.Warp(end);
    }

    float GetCurrentWaitTime()
    {
        int remainingBooks = GameObject.FindGameObjectsWithTag("Book").Length;
        float t = totalBooks > 0 ? (float)remainingBooks / totalBooks : 0f;
        return Mathf.Lerp(minWaitTime, maxWaitTime, t);
    }

    void PlayClap()
    {
        if (clapClip && audioSrc)
        {
            audioSrc.PlayOneShot(clapClip);
            myAnim.SetBool("isAngry", true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isJumpscareTriggered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("hi");
            isJumpscareTriggered = true;

            agent.isStopped = true;

            if (playerCamera)
            {
                StartCoroutine(LockCameraOnBaldi());
            }

            if (jumpscareClip && audioSrc)
            {
                audioSrc.PlayOneShot(jumpscareClip);
            }

            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    IEnumerator LockCameraOnBaldi()
    {
        while (true)
        {
            if (playerCamera == null) yield break;

            Vector3 direction = (transform.position - playerCamera.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetRotation, Time.deltaTime * 5f);

            yield return null;
        }
    }
}
