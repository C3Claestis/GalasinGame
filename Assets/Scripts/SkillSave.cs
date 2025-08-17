using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillSave : MonoBehaviour
{
    [SerializeField] Text diamondText;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip completeUp;
    [SerializeField] AudioClip notHaveDiamondUp;
    [SerializeField] GameObject warningPopUp;

    [Header("Sprite")]
    [SerializeField] Sprite[] sprites = new Sprite[5];
    [SerializeField] Image[] barUp = new Image[5];
    [SerializeField] Button[] buttonUp = new Button[5];
    [SerializeField] Sprite bgMainStats;
    [SerializeField] Sprite bgSkilStats;

    [Header("Up Panel Settings")]
    [SerializeField] Text nameStats;
    [SerializeField] Image bgStats;
    [SerializeField] Image iconStats;
    [SerializeField] Text priceTxt;

    private int currentDiamond;

    private int pathLineLevel;
    private int speedLevel;
    private int kamuflaseLevel;
    private int outAreaLevel;
    private int decreaseCovenantLevel;

    private int PricingDiamond;
    private string currentLevelUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        currentDiamond = PlayerPrefs.GetInt("DiamondCount");
        diamondText.text = currentDiamond.ToString();

        pathLineLevel = PlayerPrefs.GetInt("PathLineLevel");
        speedLevel = PlayerPrefs.GetInt("SpeedLevel");
        kamuflaseLevel = PlayerPrefs.GetInt("KamuflaseLevel");
        outAreaLevel = PlayerPrefs.GetInt("OutAreaLevel");
        decreaseCovenantLevel = PlayerPrefs.GetInt("DecreaseCovenantLevel");

        StartCurrentBar();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    #region Upgrade Stats
    public void UpgradeStatsLine()
    {
        nameStats.text = "Path Line Draw";
        bgStats.sprite = bgMainStats;
        iconStats.sprite = sprites[0];
        Pricing(pathLineLevel);
        priceTxt.text = PricingDiamond.ToString();
        currentLevelUp = "PathLine";
    }
    public void UpgradeStatsSpeed()
    {
        nameStats.text = "Character Speed";
        bgStats.sprite = bgMainStats;
        iconStats.sprite = sprites[1];
        Pricing(speedLevel);
        priceTxt.text = PricingDiamond.ToString();
        currentLevelUp = "Speed";
    }
    public void UpgradeStatsKamuflase()
    {
        nameStats.text = "Character Kamuflase";
        bgStats.sprite = bgSkilStats;
        iconStats.sprite = sprites[2];
        Pricing(kamuflaseLevel);
        priceTxt.text = PricingDiamond.ToString();
        currentLevelUp = "Kamuflase";
    }
    public void UpgradeStatsOutArea()
    {
        nameStats.text = "Character Out Area";
        bgStats.sprite = bgSkilStats;
        iconStats.sprite = sprites[3];
        Pricing(outAreaLevel);
        priceTxt.text = PricingDiamond.ToString();
        currentLevelUp = "OutArea";
    }
    public void UpgradeStatsDecreaseCovenant()
    {
        nameStats.text = "Decrease Covenant";
        bgStats.sprite = bgSkilStats;
        iconStats.sprite = sprites[4];
        Pricing(decreaseCovenantLevel);
        priceTxt.text = PricingDiamond.ToString();
        currentLevelUp = "DecreaseCovenant";
    }
    #endregion

    #region Confirm Upgrade
    public void UpgradeStats()
    {
        if (currentDiamond >= PricingDiamond)
        {
            currentDiamond -= PricingDiamond;
            diamondText.text = currentDiamond.ToString();
            PlayerPrefs.SetInt("DiamondCount", currentDiamond);

            if (currentLevelUp == "PathLine")
            {
                pathLineLevel++;
                PlayerPrefs.SetInt("PathLineLevel", pathLineLevel);
                UpdateBarStats("PathLine");
            }
            else if (currentLevelUp == "Speed")
            {
                speedLevel++;
                PlayerPrefs.SetInt("SpeedLevel", speedLevel);
                UpdateBarStats("Speed");
            }
            else if (currentLevelUp == "Kamuflase")
            {
                kamuflaseLevel++;
                PlayerPrefs.SetInt("KamuflaseLevel", kamuflaseLevel);
                UpdateBarStats("Kamuflase");
            }
            else if (currentLevelUp == "OutArea")
            {
                outAreaLevel++;
                PlayerPrefs.SetInt("OutAreaLevel", outAreaLevel);
                UpdateBarStats("OutArea");
            }
            else if (currentLevelUp == "DecreaseCovenant")
            {
                decreaseCovenantLevel++;
                PlayerPrefs.SetInt("DecreaseCovenantLevel", decreaseCovenantLevel);
                UpdateBarStats("DecreaseCovenant");
            }

            sfx.PlayOneShot(completeUp);
        }
        else
        {
            sfx.PlayOneShot(notHaveDiamondUp);
            StartCoroutine(WarningPopUp());
        }
    }

    IEnumerator WarningPopUp()
    {
        warningPopUp.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningPopUp.SetActive(false);
    }

    #endregion

    void UpdateBarStats(string jenis)
    {
        if (jenis == "PathLine")
        {
            if (pathLineLevel == 1) barUp[0].fillAmount = (float)0.2;
            else if (pathLineLevel == 2) barUp[0].fillAmount = (float)0.4;
            else if (pathLineLevel == 3) barUp[0].fillAmount = (float)0.6;
            else if (pathLineLevel == 4) barUp[0].fillAmount = (float)0.8;
            else if (pathLineLevel == 5) { barUp[0].fillAmount = (float)1.0; buttonUp[0].interactable = false; }
        }
        else if (jenis == "Speed")
        {
            if (speedLevel == 1) barUp[1].fillAmount = (float)0.2;
            else if (speedLevel == 2) barUp[1].fillAmount = (float)0.4;
            else if (speedLevel == 3) barUp[1].fillAmount = (float)0.6;
            else if (speedLevel == 4) barUp[1].fillAmount = (float)0.8;
            else if (speedLevel == 5) { barUp[1].fillAmount = (float)1.0; buttonUp[1].interactable = false; }
        }
        else if (jenis == "Kamuflase")
        {
            if (kamuflaseLevel == 1) barUp[2].fillAmount = (float)0.2;
            else if (kamuflaseLevel == 2) barUp[2].fillAmount = (float)0.4;
            else if (kamuflaseLevel == 3) barUp[2].fillAmount = (float)0.6;
            else if (kamuflaseLevel == 4) barUp[2].fillAmount = (float)0.8;
            else if (kamuflaseLevel == 5) { barUp[2].fillAmount = (float)1.0; buttonUp[2].interactable = false; }
        }
        else if (jenis == "OutArea")
        {
            if (outAreaLevel == 1) barUp[3].fillAmount = (float)0.2;
            else if (outAreaLevel == 2) barUp[3].fillAmount = (float)0.4;
            else if (outAreaLevel == 3) barUp[3].fillAmount = (float)0.6;
            else if (outAreaLevel == 4) barUp[3].fillAmount = (float)0.8;
            else if (outAreaLevel == 5) { barUp[3].fillAmount = (float)1.0; buttonUp[3].interactable = false; }
        }
        else if (jenis == "DecreaseCovenant")
        {
            if (decreaseCovenantLevel == 1) barUp[4].fillAmount = (float)0.2;
            else if (decreaseCovenantLevel == 2) barUp[4].fillAmount = (float)0.4;
            else if (decreaseCovenantLevel == 3) barUp[4].fillAmount = (float)0.6;
            else if (decreaseCovenantLevel == 4) barUp[4].fillAmount = (float)0.8;
            else if (decreaseCovenantLevel == 5) { barUp[4].fillAmount = (float)1.0; buttonUp[4].interactable = false; }
        }
    }

    void StartCurrentBar()
    {
        if (pathLineLevel == 1) barUp[0].fillAmount = (float)0.2;
        else if (pathLineLevel == 2) barUp[0].fillAmount = (float)0.4;
        else if (pathLineLevel == 3) barUp[0].fillAmount = (float)0.6;
        else if (pathLineLevel == 4) barUp[0].fillAmount = (float)0.8;
        else if (pathLineLevel == 5) { barUp[0].fillAmount = (float)1.0; buttonUp[0].interactable = false; }

        if (speedLevel == 1) barUp[1].fillAmount = (float)0.2;
        else if (speedLevel == 2) barUp[1].fillAmount = (float)0.4;
        else if (speedLevel == 3) barUp[1].fillAmount = (float)0.6;
        else if (speedLevel == 4) barUp[1].fillAmount = (float)0.8;
        else if (speedLevel == 5) { barUp[1].fillAmount = (float)1.0; buttonUp[1].interactable = false; }

        if (kamuflaseLevel == 1) barUp[2].fillAmount = (float)0.2;
        else if (kamuflaseLevel == 2) barUp[2].fillAmount = (float)0.4;
        else if (kamuflaseLevel == 3) barUp[2].fillAmount = (float)0.6;
        else if (kamuflaseLevel == 4) barUp[2].fillAmount = (float)0.8;
        else if (kamuflaseLevel == 5) { barUp[2].fillAmount = (float)1.0; buttonUp[2].interactable = false; }

        if (outAreaLevel == 1) barUp[3].fillAmount = (float)0.2;
        else if (outAreaLevel == 2) barUp[3].fillAmount = (float)0.4;
        else if (outAreaLevel == 3) barUp[3].fillAmount = (float)0.6;
        else if (outAreaLevel == 4) barUp[3].fillAmount = (float)0.8;
        else if (outAreaLevel == 5) { barUp[3].fillAmount = (float)1.0; buttonUp[3].interactable = false; }

        if (decreaseCovenantLevel == 1) barUp[4].fillAmount = (float)0.2;
        else if (decreaseCovenantLevel == 2) barUp[4].fillAmount = (float)0.4;
        else if (decreaseCovenantLevel == 3) barUp[4].fillAmount = (float)0.6;
        else if (decreaseCovenantLevel == 4) barUp[4].fillAmount = (float)0.8;
        else if (decreaseCovenantLevel == 5) { barUp[4].fillAmount = (float)1.0; buttonUp[4].interactable = false; }
    }

    int Pricing(int jenis)
    {
        switch (jenis)
        {
            case 0: //Masih Level 0
                return PricingDiamond = 10;
            case 1: //Masih Level 1
                return PricingDiamond = 20;
            case 2: //Masih Level 2
                return PricingDiamond = 30;
            case 3: //Masih Level 3
                return PricingDiamond = 40;
            case 4: //Masih Level 4
                return PricingDiamond = 50;
            default:
                return 0;
        }
    }
}
