using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDataStruct : MonoBehaviour
{
    [SerializeField] GameObject isi1, isi2, isi3, isi4, isi5, isi6, isi7, isi8;

    [Header("Scriptable Star")]
    [SerializeField] private List<StarValidasi> progressManagers = new List<StarValidasi>();

    [Header("Star Sprite")]
    [SerializeField] private Sprite starSprite;

    private List<List<Image>> starHolders = new List<List<Image>>();

    [Header("Star Hutan Belantara Setup")]
    [SerializeField] private List<Image> starHB1;
    [SerializeField] private List<Image> starHB2;
    [SerializeField] private List<Image> starHB3;
    [SerializeField] private List<Image> starHB4;
    [SerializeField] private List<Image> starHB5;
    [SerializeField] private List<Image> starHB6;
    [SerializeField] private List<Image> starHB7;
    [SerializeField] private List<Image> starHB8;
    [SerializeField] private List<Image> starHB9;
    [SerializeField] private List<Image> starHB10;

    [SerializeField] private List<Image> starHB11;
    [SerializeField] private List<Image> starHB12;
    [SerializeField] private List<Image> starHB13;
    [SerializeField] private List<Image> starHB14;
    [SerializeField] private List<Image> starHB15;
    [SerializeField] private List<Image> starHB16;
    [SerializeField] private List<Image> starHB17;
    [SerializeField] private List<Image> starHB18;
    [SerializeField] private List<Image> starHB19;
    [SerializeField] private List<Image> starHB20;

    [Header("Star Ibu Kota Setup")]
    [SerializeField] private List<Image> starIK1;
    [SerializeField] private List<Image> starIK2;
    [SerializeField] private List<Image> starIK3;
    [SerializeField] private List<Image> starIK4;
    [SerializeField] private List<Image> starIK5;
    [SerializeField] private List<Image> starIK6;
    [SerializeField] private List<Image> starIK7;
    [SerializeField] private List<Image> starIK8;
    [SerializeField] private List<Image> starIK9;
    [SerializeField] private List<Image> starIK10;

    [SerializeField] private List<Image> starIK11;
    [SerializeField] private List<Image> starIK12;
    [SerializeField] private List<Image> starIK13;
    [SerializeField] private List<Image> starIK14;
    [SerializeField] private List<Image> starIK15;
    [SerializeField] private List<Image> starIK16;
    [SerializeField] private List<Image> starIK17;
    [SerializeField] private List<Image> starIK18;
    [SerializeField] private List<Image> starIK19;
    [SerializeField] private List<Image> starIK20;

    [Header("Lock Key Image")]
    [SerializeField] private List<GameObject> lockKey;

    private void Start()
    {
        InitializeStarHolders();
        UpdateStars();
        
    }

    private void InitializeStarHolders()
    {
        // Masukkan semua starHB ke dalam 1 list
        starHolders.Add(starHB1);
        starHolders.Add(starHB2);
        starHolders.Add(starHB3);
        starHolders.Add(starHB4);
        starHolders.Add(starHB5);
        starHolders.Add(starHB6);
        starHolders.Add(starHB7);
        starHolders.Add(starHB8);
        starHolders.Add(starHB9);
        starHolders.Add(starHB10);

        starHolders.Add(starHB11);
        starHolders.Add(starHB12);
        starHolders.Add(starHB13);
        starHolders.Add(starHB14);
        starHolders.Add(starHB15);
        starHolders.Add(starHB16);
        starHolders.Add(starHB17);
        starHolders.Add(starHB18);
        starHolders.Add(starHB19);
        starHolders.Add(starHB20);

        starHolders.Add(starIK1);
        starHolders.Add(starIK2);
        starHolders.Add(starIK3);
        starHolders.Add(starIK4);
        starHolders.Add(starIK5);
        starHolders.Add(starIK6);
        starHolders.Add(starIK7);
        starHolders.Add(starIK8);
        starHolders.Add(starIK9);
        starHolders.Add(starIK10);

        starHolders.Add(starIK11);
        starHolders.Add(starIK12);
        starHolders.Add(starIK13);
        starHolders.Add(starIK14);
        starHolders.Add(starIK15);
        starHolders.Add(starIK16);
        starHolders.Add(starIK17);
        starHolders.Add(starIK18);
        starHolders.Add(starIK19);
        starHolders.Add(starIK20);
    }

    private void UpdateStars()
    {
        for (int i = 0; i < progressManagers.Count; i++)
        {
            StarValidasi pm = progressManagers[i];
            List<Image> currentStars = starHolders[i];

            if (pm.validasi1)
                currentStars[0].sprite = starSprite;

            if (pm.validasi2)
                currentStars[1].sprite = starSprite;

            if (pm.validasi3)
                currentStars[2].sprite = starSprite;
        }
    }

    public void SetUpUnlockLevel()
    {
        // Ambil level terakhir dari PlayerPrefs, default = 0
        int level = PlayerPrefs.GetInt("Level", 0);

        // Kunci semua level dulu
        for (int i = 0; i < lockKey.Count; i++)
        {
            lockKey[i].SetActive(true); // aktifkan semua gembok
            Button parentButton = lockKey[i].GetComponentInParent<Button>();
            if (parentButton != null)
                parentButton.interactable = false; // matikan tombol semua dulu
        }

        // Buka level sesuai progress
        for (int i = 0; i <= level && i < lockKey.Count; i++)
        {
            lockKey[i].SetActive(false); // hilangkan gembok
            Button parentButton = lockKey[i].GetComponentInParent<Button>();
            if (parentButton != null)
                parentButton.interactable = true; // aktifkan tombol
        }
    }
   
    #region On Off Button Site
    public void BtnSiteA()
    {
        isi1.SetActive(true);
        isi2.SetActive(true);
        isi3.SetActive(false);
        isi4.SetActive(false);
    }

    public void BtnSiteB()
    {
        isi3.SetActive(true);
        isi4.SetActive(true);
        isi1.SetActive(false);
        isi2.SetActive(false);
    }

    public void BtnSiteA2()
    {
        isi5.SetActive(true);
        isi6.SetActive(true);
        isi7.SetActive(false);
        isi8.SetActive(false);
    }

    public void BtnSiteB2()
    {
        isi7.SetActive(true);
        isi8.SetActive(true);
        isi5.SetActive(false);
        isi6.SetActive(false);
    }
    #endregion
}
