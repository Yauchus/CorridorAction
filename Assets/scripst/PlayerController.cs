using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    public float moveSpeed = 6f;
    public float gravity = -20f;
    private Vector3 velocity;

    // Ссылки на камеру
    public Transform cameraHolder; // Пустой объект с камерой, который висит за персом
    public float mouseSensitivity = 2f;

    // Плавность вращения камеры
    private float cameraPitch = 0f;
    private float cameraYaw = 0f;
    public float cameraDistance = 4f; // Расстояние камеры от персонажа

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Ищем компонент Animator на самом персонаже или на его дочерних объектах
        animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Инициализируем углы поворота камеры
        Vector3 angles = cameraHolder.eulerAngles;
        cameraPitch = angles.x;
        cameraYaw = angles.y;
    }

    void LateUpdate() // Плавное следование и вращение камеры
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraYaw += mouseX;
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -15f, 50f); // Ограничение угла по высоте

        Quaternion targetRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
        cameraHolder.rotation = targetRotation;

        // Плавное следование камеры за персонажем (Lerp)
        Vector3 targetPosition = transform.position + Vector3.up * 1.5f - (targetRotation * Vector3.forward * cameraDistance);
        cameraHolder.position = Vector3.Lerp(cameraHolder.position, targetPosition, 15f * Time.deltaTime);
    }

    void Update()
    {
        // 1. АТАКА (Клик ЛКМ)
        if (Input.GetMouseButtonDown(0))
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }

        // 2. ДВИЖЕНИЕ НА WASD (Независимо от камеры)
        float moveX = Input.GetAxis("Horizontal"); // A и D
        float moveZ = Input.GetAxis("Vertical");   // W и S

        Vector3 forward = cameraHolder.forward;
        Vector3 right = cameraHolder.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveZ + right * moveX;

        if (move.magnitude > 0.1f)
        {
            // Плавный поворот персонажа в сторону движения
            Quaternion targetCharRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetCharRotation, 10f * Time.deltaTime);

            // Двигаем персонажа
            controller.Move(move * moveSpeed * Time.deltaTime);

            // Передаем скорость в аниматор (бег)
            if (animator != null)
            {
                animator.SetFloat("Speed", move.magnitude * moveSpeed);
            }
        }
        else
        {
            // Если стоим на месте, сбрасываем скорость в аниматоре
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
        }

        // 3. ГРАВИТАЦИЯ
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}