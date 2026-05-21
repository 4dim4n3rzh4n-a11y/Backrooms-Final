using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject hudUI;
    public GameObject armUI;
    public CameraControlller cameraController;

    private bool isPaused;

    public bool IsPaused => isPaused;

    void Start()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // курсор
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        pauseUI.SetActive(isPaused);
        hudUI.SetActive(!isPaused);

        if (armUI != null)
            armUI.SetActive(!isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
        AudioListener.pause = isPaused;

        if (cameraController != null)
            cameraController.enabled = !isPaused;
    }

    public void Resume()
    {
        isPaused = false;

        pauseUI.SetActive(false);
        hudUI.SetActive(true);

        if (armUI != null)
            armUI.SetActive(true);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (cameraController != null)
            cameraController.enabled = true;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}