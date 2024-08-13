using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : UIMenu
{
    public static bool GameIsPaused { get; private set; } = false;
    public static PauseMenu Instance = null;
    [SerializeField] Image Background = null;

    private void Awake()
    {
        Instance = this;
    }

    public static void Pause()
    {
        GameIsPaused = !GameIsPaused;
        if (GameIsPaused)
        {
            Time.timeScale = 0f;
            Instance.gameObject.SetActive(true);
            Instance.Background.enabled = true;
        }
        else        
            Instance.OnResume();
    }

    public void OnResume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        Instance.gameObject.SetActive(false);
        Instance.Background.enabled = false;
        MainMenuManager.DisableAllMenuItems();
    }

    public void OnQuit()
    {
        OnResume();
        SceneManager.LoadScene("MainMenu");
    }
}
