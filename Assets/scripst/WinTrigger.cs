using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour

{
    void Start()
    {
        Debug.Log("Плита успешно инициализирована и работает!");
    }
    [Header("Перетащи сюда WinPanel руками")]
    public GameObject winPanel;

    private bool hasWon = false;

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошел игрок
        if (!hasWon && other.CompareTag("Player"))
        {
            hasWon = true;
            Debug.Log("Игрок наступил на плиту победы!");

            ActivateWinPanel();
        }
    }

    void ActivateWinPanel()
    {
        // 1. Если объект назначен в инспекторе
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Debug.Log("Победная панель включена через ссылку!");
        }
        else
        {
            // 2. Если не назначен — ищем во всей сцене объект с любым UI Image/Text внутри него, который похож на панель
            // Или ищем по точному имени среди ВСЕХ объектов (даже выключенных)
            Transform[] allObjects = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (Transform t in allObjects)
            {
                if (t.name == "WinPanel" && t.gameObject.scene.isLoaded)
                {
                    winPanel = t.gameObject;
                    winPanel.SetActive(true);
                    Debug.Log("Победная панель найдена глобально и включена!");
                    break;
                }
            }
        }

        if (winPanel == null)
        {
            Debug.LogError("КРИТИЧЕСКАЯ ОШИБКА: На сцене вообще нет объекта с именем 'WinPanel'!");
        }

        // Останавливаем время и показываем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
    
}