using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject gameOverUI;

    [Header("Level Buttons")]
    public Button btnLevel2; 

    private static bool isMuted = false;

    void Awake() 
    { 
        instance = this; 
    }

    void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
        
        CheckLevelProgress();
    }

    public void CheckLevelProgress()
    {
        int levelPassed = PlayerPrefs.GetInt("LevelPassed", 0);

        if (btnLevel2 != null)
        {
            if (levelPassed >= 1) {
                btnLevel2.interactable = true;
            } else {
                btnLevel2.interactable = false;
            }
        }
    }

    public void StartGame() 
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        CheckLevelProgress(); 
    }

    public void BackToMenu() 
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
    }

    public void LoadLevel1() { Time.timeScale = 1f; SceneManager.LoadScene("LV1"); }
    public void LoadLevel2() { Time.timeScale = 1f; SceneManager.LoadScene("LV2"); }

    public void ToggleSoundButton() 
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

    // --- เอาระบบ Game Over กลับมาแล้ว ---
    public void GameOver()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenuScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Game"); 
    }
    
    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();
        CheckLevelProgress();
    }

    public void QuitGame() 
    { 
        Application.Quit(); 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}