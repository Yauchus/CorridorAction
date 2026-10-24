using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод для запуска игры (переключает на твою игровую сцену)
    public void PlayGame()
    {
        // Укажи здесь точное название твоей основной игровой сцены в кавычках!
        SceneManager.LoadScene("SampleScene"); // Замени "SampleScene" на имя вашей игровой сцены
    }

    // Метод для выхода из игры
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}