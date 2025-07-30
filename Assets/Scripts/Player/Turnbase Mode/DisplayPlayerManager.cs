using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerManager : MonoBehaviour
{
    [SerializeField] private DrawPathMovement[] players;
    [SerializeField] private Button[] btnDisplayPlayer;

    private Color[] colors;

    private void Awake()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        TakeImageDisplay();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Pastikan colors memiliki ukuran yang sama dengan players
        colors = new Color[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                colors[i] = players[i].GetComponent<MeshRenderer>().material.color;
            }
        }

        ApplyColorsToButtons();
        SetupPlayerButtonEvents();
    }

    private void TakeImageDisplay()
    {
        if (btnDisplayPlayer == null || btnDisplayPlayer.Length == 0)
        {
            GameObject displayParent = GameObject.Find("DisplayPlayer");
            if (displayParent != null && displayParent.transform.childCount > 0)
            {
                int childCount = displayParent.transform.childCount;
                btnDisplayPlayer = new Button[childCount];
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = displayParent.transform.GetChild(i);
                    btnDisplayPlayer[i] = child.GetComponent<Button>();
                }
            }
        }
    }

    private void ApplyColorsToButtons()
    {
        for (int i = 0; i < btnDisplayPlayer.Length && i < colors.Length; i++)
        {
            Image img = btnDisplayPlayer[i].GetComponent<Image>();
            if (img != null)
            {
                img.color = colors[i];
            }
        }
    }

    public void SetupPlayerButtonEvents()
    {
        for (int i = 0; i < btnDisplayPlayer.Length && i < players.Length; i++)
        {
            int index = i; // Penting agar closure benar
            if (btnDisplayPlayer[index] != null && players[index] != null)
            {
                btnDisplayPlayer[index].onClick.RemoveAllListeners();
                btnDisplayPlayer[index].onClick.AddListener(() => players[index].SetIsSelected(true));
            }
        }
    }
}
