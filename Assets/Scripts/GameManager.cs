using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }

    Enemy[] enemies;
    DrawPathMovement[] players;

    [Header("Audio Settings")]
    [SerializeField] AudioSource audioSourcePlayer;
    [SerializeField] AudioSource audioSourceEnemy;
    [SerializeField] AudioClip movePlayerSFX;
    [SerializeField] AudioClip enemyPlayerSFX;

    [Header("UI Elements")]
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

    void Update()
    {
        HandlePlayerMoveSFX();
        HandleEnemyMoveSFX();
    }

    #region Audio Player Move
    private void HandlePlayerMoveSFX()
    {
        if (players == null || players.Length == 0) return;

        bool anyMoving = false;
        bool allIdle = true;

        foreach (var player in players)
        {
            if (player != null)
            {
                if (player.GetMoving())
                {
                    anyMoving = true;   // ada yg gerak
                    allIdle = false;    // berarti tidak semua idle
                }
            }
        }

        // Jika ada yang bergerak → play SFX
        if (anyMoving)
        {
            if (!audioSourcePlayer.isPlaying || audioSourcePlayer.clip != movePlayerSFX)
            {
                audioSourcePlayer.clip = movePlayerSFX;
                audioSourcePlayer.loop = true;
                audioSourcePlayer.pitch = 2f;
                audioSourcePlayer.Play();
            }
        }
        // Jika semuanya idle → stop SFX
        else if (allIdle)
        {
            if (audioSourcePlayer.isPlaying && audioSourcePlayer.clip == movePlayerSFX)
            {
                audioSourcePlayer.Stop();
            }
        }
    }
    #endregion

    #region Audio Enemy Move
    private void HandleEnemyMoveSFX()
    {
        if (enemies == null || enemies.Length == 0) return;

        bool anyMoving = false;
        bool allIdle = true;

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                Animator anim = enemy.GetComponentInChildren<Animator>();
                if (anim != null)
                {
                    bool isMoving = anim.GetCurrentAnimatorStateInfo(0).IsName("Move");

                    if (isMoving)
                    {
                        anyMoving = true;   // ada yg gerak
                        allIdle = false;    // berarti tidak semua idle
                    }
                }
            }
        }

        // Jika ada minimal 1 enemy lagi "Move"
        if (anyMoving)
        {
            if (!audioSourceEnemy.isPlaying || audioSourceEnemy.clip != enemyPlayerSFX)
            {
                audioSourceEnemy.clip = enemyPlayerSFX;
                audioSourceEnemy.loop = true;
                audioSourceEnemy.Play();
            }
        }
        // Jika semua enemy tidak "Move"
        else if (allIdle)
        {
            if (audioSourceEnemy.isPlaying && audioSourceEnemy.clip == enemyPlayerSFX)
            {
                audioSourceEnemy.Stop();
            }
        }
    }
    #endregion

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

    public bool IsAboveOneMinute() => currentTime > 60f;
    public bool IsAboveTwoMinutes() => currentTime > 120f;
    public bool IsAboveThirtySeconds() => currentTime > 30f;

}
