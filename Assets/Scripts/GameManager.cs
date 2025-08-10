using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }

    Enemy[] enemies;
    DrawPathMovement[] players;

    [SerializeField] Text defenderTxt;
    [SerializeField] Text attackerTxt;

    [SerializeField] Text timerTxt;

    [SerializeField] GameObject nextRoundPanel;

    [SerializeField] private float countdownTime; // waktu awal timer (detik)

    private float currentTime;
    private int defenderScore = 0;
    private int attackerScore = 0;

    bool isUpdated = false;

    void Awake()
    {
        // Setup Singleton Instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // mencegah duplikat
            return;
        }
        Instance = this;

        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defenderTxt.text = $"0";
        attackerTxt.text = $"0";
        currentTime = countdownTime; // set waktu awal
        StartCoroutine(TimerCoroutine());

        StartCoroutine(CheckPlayersNameCoroutine());
    }

    #region Timer
    // Fungsi untuk update tampilan timer
    void CountdownTimer(float time)
    {
        if (time <= 0)
        {
            timerTxt.text = "Time's up!";
            return;
        }

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerTxt.text = $"{minutes:00}:{seconds:00}";
    }

    IEnumerator TimerCoroutine()
    {
        currentTime = countdownTime;
        while (currentTime > 0)
        {
            CountdownTimer(currentTime);
            yield return null;
            currentTime -= Time.deltaTime;
        }
        currentTime = 0;
        CountdownTimer(currentTime);
    }
    #endregion

    #region Skor
    public void UpdateDefenderCatch(int score)
    {
        defenderScore += score;
        defenderTxt.text = $"{defenderScore}";

        StopMoveEnemys();
        StopMovePlayers();
        StopAllCoroutines();
        nextRoundPanel.SetActive(true);
    }

    public void UpdateAttackerCatch(int score)
    {
        attackerScore += score;
        attackerTxt.text = $"{attackerScore}";  
    }

    private void RestartGame()
    {
        StartCoroutine(TimerCoroutine());
    }
    #endregion

    #region Manage Functions Enemy and Player
    private void StopMoveEnemys()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.SetCanMove(false);
        }
    }

    private void StopMovePlayers()
    {
        foreach (var player in players)
        {
            if (player != null)
                player.SetCanMove(false);
        }
    }

    IEnumerator CheckPlayersNameCoroutine()
    {
        while (true)
        {
            bool allChampions = true;
            foreach (DrawPathMovement player in players)
            {
                if (player.name != "Champion")
                {
                    allChampions = false;
                    break;
                }
            }

            if (allChampions && !isUpdated)
            {              
                isUpdated = true;
            }

            yield return new WaitForSeconds(1f); // periksa setiap 1 detik
        }
    }
    #endregion

    #region Next Round
    public void NextRound()
    {
        nextRoundPanel.SetActive(false);
        RestartGame();
        isUpdated = false;

        foreach (var player in players)
        {
            if (player != null)
                player.ResetPlayer();
        }

        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.SetCanMove(true);
        }
    }
    #endregion

    public int GetDefenderScore()
    {
        return defenderScore;
    }

    public int GetAttackerScore()
    {
        return attackerScore;
    }
}
