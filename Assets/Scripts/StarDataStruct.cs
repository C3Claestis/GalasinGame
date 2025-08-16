using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDataStruct : MonoBehaviour
{
    [Header("Scriptable Star")]
    [SerializeField] private List<StarValidasi> progressManagers = new List<StarValidasi>();

    [Header("Star Sprite")]
    [SerializeField] private Sprite starSprite;

    private List<List<Image>> starHolders = new List<List<Image>>();

    [Header("Manual Setup for Inspector")]
    [SerializeField] private List<Image> starHB1;
    [SerializeField] private List<Image> starHB2;
    [SerializeField] private List<Image> starHB3;
    [SerializeField] private List<Image> starHB4;
    [SerializeField] private List<Image> starHB5;
    // [SerializeField] private List<Image> starHB6;
    // [SerializeField] private List<Image> starHB7;
    // [SerializeField] private List<Image> starHB8;
    // [SerializeField] private List<Image> starHB9;
    // [SerializeField] private List<Image> starHB10;

    private void Start()
    {
        // Masukkan semua starHB ke dalam 1 list
        starHolders.Add(starHB1);
        starHolders.Add(starHB2);
        starHolders.Add(starHB3);
        starHolders.Add(starHB4);
        starHolders.Add(starHB5);
        // starHolders.Add(starHB6);
        // starHolders.Add(starHB7);
        // starHolders.Add(starHB8);
        // starHolders.Add(starHB9);
        // starHolders.Add(starHB10);

        UpdateStars();
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
}
