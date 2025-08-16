using UnityEngine;
using UnityEngine.UI;

public class CovenantManager : MonoBehaviour
{
    [Header("Progress and UI Manager")]
    [SerializeField] ProgressManager progressManager;
    [SerializeField] StarValidasi starValidasi;

    [Header("Covenant Settings")]
    [SerializeField] int enemyRound;
    [SerializeField] Text covenantText;
    [SerializeField] Text loseText;

    [Header("Panel Settings")]
    [SerializeField] Text titleTxt;
    [SerializeField] Text descriptionTxt1;
    [SerializeField] Text descriptionTxt2;
    [SerializeField] Text descriptionTxt3;
    [SerializeField] Image[] starImage;

    [Header("Star Sprites")]
    [SerializeField] Sprite filledStar;

    UIManager uiManager;
    GameManager gameManager;
    private DrawPathMovement[] players;
    private int currentCovenantId;

    private int countPlayerFinish = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        covenantText.text = players.Length.ToString();
        loseText.text = enemyRound.ToString();

        InitializeProgress();
        uiManager = GetComponent<UIManager>();
        gameManager = GetComponent<GameManager>();
        SetUpStar();
    }

    private void SetUpStar()
    {
        // Sinkronisasi nilai StarValidasi
        progressManager.isProgress1Complete = starValidasi.validasi1;
        progressManager.isProgress2Complete = starValidasi.validasi2;
        progressManager.isProgress3Complete = starValidasi.validasi3;

        for (int i = 0; i < starImage.Length; i++)
        {
            if ((i == 0 && starValidasi.validasi1) ||
                (i == 1 && starValidasi.validasi2) ||
                (i == 2 && starValidasi.validasi3))
            {
                starImage[i].sprite = filledStar;
            }
        }
    }

    #region Trigger UI Finish and Lose
    public void CheckPlayerFinished()
    {
        gameManager.UpdateAttackerCatch(1);
        countPlayerFinish++;

        if (countPlayerFinish >= players.Length)
        {
            uiManager.DisplayCompletion();
            CheckCovenantIndex();
        }
    }

    private void CheckCovenant()
    {
        if (currentCovenantId >= enemyRound)
        {
            uiManager.DisplayGameOver();
        }
    }
    #endregion

    #region Function For Star Update
    private void OnEnable()
    {
        ProgressManager.OnStarUpdated += UpdateStars;
    }

    private void OnDisable()
    {
        ProgressManager.OnStarUpdated -= UpdateStars;
    }

    private void UpdateStars(int _)
    {
        bool[] progressStatus = {
        progressManager.isProgress1Complete,
        progressManager.isProgress2Complete,
        progressManager.isProgress3Complete
    };

        for (int i = 0; i < starImage.Length; i++)
        {
            if (progressStatus[i]) starImage[i].sprite = filledStar;
        }
    }
    #endregion

    private void CheckCovenantIndex()
    {
        int[] thresholds = { 1, 3, 5, 7 };
        ProgressType[] progressTypes =
        {
        ProgressType.SelesaiSebelum1Covenant,
        ProgressType.SelesaiSebelum3Covenant,
        ProgressType.SelesaiSebelum5Covenant,
        ProgressType.SelesaiSebelum7Covenant
    };

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (enemyRound < thresholds[i])
            {
                for (int j = i; j < progressTypes.Length; j++)
                {
                    ProgressSystem.Instance.CompleteProgressByType(progressTypes[j]);
                }
                break; // keluar setelah menemukan batas pertama
            }
        }
    }

    private void InitializeProgress()
    {
        if (progressManager == null)
        {
            Debug.LogError("ProgressManager is not assigned!");
            return;
        }

        // Initialize progress values
        titleTxt.text = progressManager.title;
        descriptionTxt1.text = progressManager.GetProgress1Description();
        descriptionTxt2.text = progressManager.GetProgress2Description();
        descriptionTxt3.text = progressManager.GetProgress3Description();
    }

    public void SetCovenant(int Covenant)
    {
        currentCovenantId += Covenant;
        CheckCovenant();
    }

    public ProgressManager GetProgressManager() => progressManager;
    public StarValidasi GetStarValidasi() => starValidasi;
}
