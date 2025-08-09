using UnityEngine;
using UnityEngine.UI;

public class CovenantManager : MonoBehaviour
{
    [Header("Progress and UI Manager")]
    [SerializeField] ProgressManager progressManager;

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
    private DrawPathMovement[] players;
    private int currentCovenantId;

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
        progressManager.ResetProgress();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        ProgressManager.OnStarUpdated += UpdateStars;
    }

    private void OnDisable()
    {
        ProgressManager.OnStarUpdated -= UpdateStars;
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

    private void CheckCovenant()
    {
        if (currentCovenantId >= enemyRound)
        {
            uiManager.DisplayGameOver();
        }
    }

    public void SetCovenant(int Covenant)
    {
        currentCovenantId += Covenant;
        CheckCovenant();
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

    public ProgressManager GetProgressManager() => progressManager;
}
