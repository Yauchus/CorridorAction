using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    public float moveSpeed = 6f;
    public float gravity = -20f;
    private Vector3 velocity;

    // Combat
    public GameObject attackHitbox;

    [Header("Shield Settings")]
    public GameObject shieldEffect;
    public bool isBlocking;

    // Camera settings
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    private float cameraPitch = 0f;
    private float cameraYaw = 0f;
    public float cameraDistance = 4f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize camera angles
        if (cameraHolder != null)
        {
            Vector3 angles = cameraHolder.eulerAngles;
            cameraPitch = angles.x;
            cameraYaw = angles.y;
        }
    }

    void LateUpdate()
    {
        if (cameraHolder == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraYaw += mouseX;
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -15f, 50f); // Limit vertical pitch angle

        Quaternion targetRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
        cameraHolder.rotation = targetRotation;

        // Smoothly follow the player (Lerp)
        Vector3 targetPosition = transform.position + Vector3.up * 1.5f - (targetRotation * Vector3.forward * cameraDistance);
        cameraHolder.position = Vector3.Lerp(cameraHolder.position, targetPosition, 15f * Time.deltaTime);
    }

    void Update()
    {
        // 1. Shield / Block (Hold RMB)
        if (Input.GetMouseButton(1))
        {
            isBlocking = true;
            if (shieldEffect != null) shieldEffect.SetActive(true);
        }
        else
        {
            isBlocking = false;
            if (shieldEffect != null) shieldEffect.SetActive(false);
        }

        // 2. Attack (Click LMB) - only works if not blocking
        if (Input.GetMouseButtonDown(0) && !isBlocking)
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            StartCoroutine(ActivateHitbox());
        }

        // 3. Movement (WASD)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 forward = cameraHolder != null ? cameraHolder.forward : transform.forward;
        Vector3 right = cameraHolder != null ? cameraHolder.right : transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveZ + right * moveX;

        if (move.magnitude > 0.1f)
        {
            // Smoothly rotate character towards movement direction
            Quaternion targetCharRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetCharRotation, 10f * Time.deltaTime);

            // Move the character
            controller.Move(move * moveSpeed * Time.deltaTime);

            // Pass speed to animator
            if (animator != null)
            {
                animator.SetFloat("Speed", move.magnitude * moveSpeed);
            }
        }
        else
        {
            // Reset animator speed when stationary
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
        }

        // 4. Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Coroutine to activate attack hitbox for a short duration
    IEnumerator ActivateHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
            yield return new WaitForSeconds(0.3f); // Hitbox is active for 0.3 seconds
            attackHitbox.SetActive(false);
        }
    }
}


