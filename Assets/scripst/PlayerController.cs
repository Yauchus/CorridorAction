using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 6f;
    public float gravity = -20f;
    private Vector3 velocity;

    // Настройки для камеры
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    private float cameraPitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Скрываем курсор мыши и фиксируем в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. ВРАЩЕНИЕ КАМЕРЫ И ПЕРСОНАЖА МЫШКОЙ
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Вращаем камеру вверх-вниз
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -30f, 50f); // Ограничение углов обзора
        cameraHolder.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        // Вращаем всего персонажа влево-вправо по оси Y
        transform.Rotate(Vector3.up * mouseX);

        // 2. ДВИЖЕНИЕ НА WASD
        float moveX = Input.GetAxis("Horizontal"); // A и D
        float moveZ = Input.GetAxis("Vertical");   // W и S

        // Направление движения относительно того, куда смотрит персонаж
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 3. ГРАВИТАЦИЯ (чтобы не висел в воздухе)
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}