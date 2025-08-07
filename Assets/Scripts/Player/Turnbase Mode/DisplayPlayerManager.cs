using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerManager : MonoBehaviour
{
    [SerializeField] private GoThroughtFunction goThroughtFunction;
    [SerializeField] private DrawPathMovement[] players;
    [SerializeField] private Button[] btnDisplayPlayer;

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
                for (int i = 1; i < childCount; i++)
                {
                    Transform child = displayParent.transform.GetChild(i);
                    btnDisplayPlayer[i - 1] = child.GetComponent<Button>();
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
        foreach (var btn in btnDisplayPlayer)
        {
            if (btn != null)
                btn.interactable = true;
        }
    }
}
