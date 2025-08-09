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

    
    private CovenantManager covenantManager;
    private ProgressManager progressManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        covenantManager = GetComponent<CovenantManager>();

        progressManager = covenantManager.GetProgressManager();
        InitializeProgress();
    }

    // Update is called once per frame
    void Update()
    {

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
