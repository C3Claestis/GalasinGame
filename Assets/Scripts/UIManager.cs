using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverPanel;

    [Header("Game Over Settings")]
    [SerializeField] Color gameOverTextColor = Color.white;
    [SerializeField] Color gameOverBackgroundColor = Color.red;
    [SerializeField] Image[] gameOverBackgroundImage = new Image[2];
    [SerializeField] Text[] gameOverText = new Text[5];
    [SerializeField] Image[] starImage = new Image[3];

    [SerializeField] Sprite filledStar;

    private CovenantManager covenantManager;
    private ProgressManager progressManager;

    void Awake()
    {
        covenantManager = GetComponent<CovenantManager>();

        progressManager = covenantManager.GetProgressManager();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        InitializeProgress();
    }

    void OnEnable()
    {
        ProgressManager.OnStarUpdated += StarActive;
    }

    void OnDisable()
    {
        ProgressManager.OnStarUpdated -= StarActive;
    }

    public void StarActive(int _)
    {
        bool[] progressStatus = {
            progressManager.isProgress1Complete,
            progressManager.isProgress2Complete,
            progressManager.isProgress3Complete
        };

        for (int i = 0; i < starImage.Length; i++)
        {
            if (progressStatus[i])
                starImage[i].sprite = filledStar;
        }
    }

    public void DisplayCompletion()
    {
        // Logic to display completion UI
        gameOverPanel.SetActive(true);

        gameOverText[0].text = "Complete";
        ProgressSystem.Instance.CompleteProgressByType(ProgressType.BerhasilGoThrought);

        if (GameManager.Instance.IsAboveOneMinute()) ProgressSystem.Instance.CompleteProgressByType(ProgressType.GoThrought1Menit);
        if (GameManager.Instance.IsAboveTwoMinutes()) ProgressSystem.Instance.CompleteProgressByType(ProgressType.GoThrought2Menit);
        if (GameManager.Instance.IsAboveThirtySeconds()) ProgressSystem.Instance.CompleteProgressByType(ProgressType.GoThrought30Detik);
    }

    public void DisplayGameOver()
    {
        gameOverPanel.SetActive(true);

        foreach (Image img in gameOverBackgroundImage)
        {
            img.color = gameOverBackgroundColor;
        }

        for (int i = 0; i < gameOverText.Length; i++)
        {
            gameOverText[i].color = gameOverTextColor;
        }

        gameOverText[0].text = "You Lose";
    }

    public void BtnPause()
    {
        // Logic to pause the game
        Debug.Log("Game Paused");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze the game time
    }

    public void BtnResume()
    {
        // Logic to resume the game
        Debug.Log("Game Resumed");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Unfreeze the game time
        Debug.Log("Game Resumed");
    }

    public void BtnRestart()
    {
        // Logic to restart the game
        Debug.Log("Game Restarted");
        // Assuming you have a method to reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Ensure time is unpaused
    }

    public void BtnExit()
    {
        // Logic to exit the game
        Debug.Log("Game Exited");
        // Assuming you have a method to reload the current scene
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    private void InitializeProgress()
    {
        if (progressManager == null)
        {
            Debug.LogError("ProgressManager is not assigned!");
            return;
        }

        // Initialize progress values
        gameOverText[1].text = progressManager.title;
        gameOverText[2].text = progressManager.GetProgress1Description();
        gameOverText[3].text = progressManager.GetProgress2Description();
        gameOverText[4].text = progressManager.GetProgress3Description();
    }
}
