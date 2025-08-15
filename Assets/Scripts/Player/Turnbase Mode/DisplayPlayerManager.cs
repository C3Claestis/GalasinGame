using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerManager : MonoBehaviour
{
    [SerializeField] private GoThroughtFunction goThroughtFunction;
    [SerializeField] private DrawPathMovement[] players;
    [SerializeField] private Button[] btnDisplayPlayer;

    [SerializeField] private Image[] frameBtn;

    private Color[] colors;

    private void Awake()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        if (goThroughtFunction == null)
        {
            goThroughtFunction = FindAnyObjectByType<GoThroughtFunction>();
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
                colors[i] = players[i].GetColor();
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
            if (displayParent != null && displayParent.transform.childCount > 1)
            {
                int childCount = displayParent.transform.childCount;
                btnDisplayPlayer = new Button[childCount - 1];
                frameBtn = new Image[childCount - 1];

                for (int i = 1; i < childCount; i++)
                {
                    Transform child = displayParent.transform.GetChild(i);
                    btnDisplayPlayer[i - 1] = child.GetComponent<Button>();
                    frameBtn[i - 1] = child.GetChild(0).GetComponent<Image>();
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
                btnDisplayPlayer[index].onClick.AddListener(() =>
                {
                    // Set semua player selain index ini menjadi tidak selected
                    for (int j = 0; j < players.Length; j++)
                    {
                        if (j != index && players[j] != null)
                            players[j].SetIsSelected(false);
                    }
                    players[index].SetIsSelected(true);
                    players[index].SetCanMove(false);
                    btnDisplayPlayer[index].interactable = false; // Button jadi tidak bisa diklik lagi
                    goThroughtFunction.CheckAllButtonsAndActivateGoThrought();

                    // Ubah alpha menjadi 150
                    if (frameBtn[index] != null)
                    {
                        Color c = frameBtn[index].color;
                        c.a = 150f / 255f;
                        frameBtn[index].color = c;
                    }
                });
            }
        }
    }

    public void ReactivateButtonsIfAllPlayersCanMoveWithDelay()
    {
        bool allCanMove = true;
        foreach (var player in players)
        {
            if (player == null || !player.GetCanMove())
            {
                allCanMove = false;
                break;
            }
        }

        if (allCanMove)
        {
            StartCoroutine(ReactivateButtonsCoroutine());
        }
    }

    private System.Collections.IEnumerator ReactivateButtonsCoroutine()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < btnDisplayPlayer.Length; i++)
        {
            if (btnDisplayPlayer[i] != null)
            {
                btnDisplayPlayer[i].interactable = true;

                // Kembalikan alpha jadi 255
                if (frameBtn[i] != null)
                {
                    Color c = frameBtn[i].color;
                    c.a = 1f; // 255 / 255
                    frameBtn[i].color = c;
                }
            }
        }
    }
}
