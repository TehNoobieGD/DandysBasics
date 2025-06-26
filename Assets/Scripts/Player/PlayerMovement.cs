using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 6.5f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 575f;
    public Camera playerCamera;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public LayerMask groundMask;

    [Header("Stamina Settings")]
    public RawImage staminaBarImage;
    public float maxStamina = 5f;
    public float staminaDrainRate = 1f;
    public float staminaRegenIdleRate = 1f;
    public float staminaRegenWalkRate = 0.25f;

    private float currentStamina;
    private bool isRunningAllowed = true;

    private float rotationY = 0f;
    private CharacterController controller;
    private bool isAzerty = false;

    private Vector3 velocity;
    private bool isGrounded;

    private RectTransform staminaRect;

    // UI parameters for stamina bar
    private float fullWidth = 381.11f;
    private float fullPosX = 0.6927795f;
    private float emptyPosX = -189.86f;
    private float posY = -15.822f;
    private float height = 31.643f;

    private int previousBookCount;

    // Nouveau : état si une vidéo est en train de jouer
    private bool isVideoPlaying = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        SetCursorLocked(true);

        currentStamina = maxStamina;

        if (staminaBarImage != null)
            staminaRect = staminaBarImage.GetComponent<RectTransform>();

        // Détection clavier AZERTY ou QWERTY
        isAzerty = IsAzertyKeyboard();

        previousBookCount = GameObject.FindGameObjectsWithTag("Book").Length;
    }

    void Update()
    {
        UpdateCursorLockState();

        if (!PauseManager.PauseGame && !isVideoPlaying)
        {
            HandleLook();
            HandleMovement();
            UpdateStamina();
            UpdateStaminaBarUI();
            CheckBookDisappearance();
        }
        // Sinon si vidéo joue, on bloque tout sauf curseur visible
    }

    bool IsAzertyKeyboard()
    {
        return Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.W);
    }

    void UpdateCursorLockState()
    {
        bool wasVideoPlaying = isVideoPlaying;
        isVideoPlaying = false;

        VideoPlayer[] allVideos = GameObject.FindObjectsOfType<VideoPlayer>();
        foreach (VideoPlayer vp in allVideos)
        {
            if (vp.isPlaying)
            {
                isVideoPlaying = true;
                break;
            }
        }

        bool shouldUnlockCursor = PauseManager.PauseGame || isVideoPlaying;

        if (shouldUnlockCursor)
        {
            if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
            {
                SetCursorLocked(false);
            }
        }
        else
        {
            if (Cursor.lockState != CursorLockMode.Locked || Cursor.visible)
            {
                SetCursorLocked(true);
            }
        }
    }

    void SetCursorLocked(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleLook()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseXRaw = Input.GetAxis("Mouse X");
            float mouseX = (Input.GetKey(KeyCode.Space) ? -mouseXRaw : mouseXRaw) * mouseSensitivity * Collector.settings.mouseSensitivity * Time.deltaTime;

            rotationY += mouseX;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

            if (playerCamera != null)
            {
                float cameraYRotation = rotationY + (Input.GetKey(KeyCode.Space) ? 180f : 0f);
                playerCamera.transform.rotation = Quaternion.Euler(0f, cameraYRotation, 0f);
            }
        }
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (isAzerty)
        {
            moveX = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.Q) ? 1 : 0);
            moveZ = (Input.GetKey(KeyCode.Z) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        }
        else
        {
            moveX = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
            moveZ = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        }

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.Normalize();

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        float targetSpeed = walkSpeed;
        bool tryingToRun = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f && move.magnitude > 0.1f;

        if (tryingToRun && isRunningAllowed)
        {
            targetSpeed = runSpeed;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        if (currentStamina <= 0f)
        {
            targetSpeed = walkSpeed;
            isRunningAllowed = false;
        }
        else if (currentStamina > 0.1f)
        {
            isRunningAllowed = true;
        }

        Vector3 finalMove = move * targetSpeed + velocity;
        controller.Move(finalMove * Time.deltaTime);
    }

    void UpdateStamina()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                        Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q);

        if (!Input.GetKey(KeyCode.LeftShift) || currentStamina <= 0f)
        {
            if (isMoving)
                currentStamina += staminaRegenWalkRate * Time.deltaTime;
            else
                currentStamina += staminaRegenIdleRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    void UpdateStaminaBarUI()
    {
        if (staminaRect != null)
        {
            float percent = Mathf.Clamp01(currentStamina / maxStamina);

            float width = Mathf.Lerp(0f, fullWidth, percent);
            float posX = Mathf.Lerp(emptyPosX, fullPosX, percent);

            staminaRect.sizeDelta = new Vector2(width, height);
            staminaRect.anchoredPosition = new Vector2(posX, posY);
        }
    }

    void CheckBookDisappearance()
    {
        int currentBookCount = GameObject.FindGameObjectsWithTag("Book").Length;

        if (currentBookCount < previousBookCount)
        {
            currentStamina = maxStamina;
        }

        previousBookCount = currentBookCount;
    }
}
