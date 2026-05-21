using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource clickSound;

    public void PlayGame()
    {
        clickSound.Play();
        Invoke("LoadGame", 0.3f);
    }

    void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        clickSound.Play();
        Invoke("QuitGame", 0.3f);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}