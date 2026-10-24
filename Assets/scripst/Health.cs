using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health parametrs")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Who?")]
    public bool isPlayer = false;

    [Header("UI (for player)")]
    public Slider hpSlider;
    public GameObject gameOverPanel;
    public GameObject winPanel; 

    void Start()
    {
        currentHealth = maxHealth;
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (winPanel == null)
        {
            Transform canvasTransform = FindObjectOfType<Canvas>()?.transform;
            if (canvasTransform != null)
            {
                Transform found = canvasTransform.Find("WinPanel");
                if (found != null) winPanel = found.gameObject;
            }
        }

        if (winPanel != null) winPanel.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        if (isPlayer)
        {
            PlayerController player = GetComponent<PlayerController>();
            if (player != null && player.isBlocking)
            {
                Debug.Log("Атака заблокирована щитом!");
                return;
            }
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (hpSlider != null)
        {
            hpSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isPlayer)
        {
            Debug.Log("GAME OVER");
            if (gameOverPanel != null) gameOverPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("Враг уничтожен");
            Destroy(gameObject);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Выход из игры");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}