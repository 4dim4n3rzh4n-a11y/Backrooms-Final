using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitTrigger : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"💥 Кто-то коснулся триггера: {other.name}, Тег объекта: {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("🎉 Игрок зашел в триггер! Загружаем меню...");
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}