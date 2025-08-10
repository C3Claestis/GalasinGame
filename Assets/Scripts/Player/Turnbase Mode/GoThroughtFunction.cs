using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class GoThroughtFunction : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform displayPlayerManager;
    [SerializeField] private DisplayPlayerManager display;
    [SerializeField] private Button btnGoThrought;
    [SerializeField] private DrawPathMovement[] players;

    private int countNotMovePlayer;

    private void Awake()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        if (displayPlayerManager == null)
        {
            displayPlayerManager = GameObject.Find("DisplayPlayer").transform;
        }

        if (display == null)
        {
            display = FindAnyObjectByType<DisplayPlayerManager>();
        }

        countNotMovePlayer = players.Length;
    }

    public void GoThroughAllPlayers()
    {
        foreach (var player in players)
        {
            if (player != null)
            {
                player.SetCanMove(true);
                player.SetIsSelected(false);
                player.SetIsDrawing(false);
            }
        }

        cinemachineCamera.Target.TrackingTarget = transform;

        display.ReactivateButtonsIfAllPlayersCanMoveWithDelay();
        Invoke(nameof(CheckMovingPlayer), 1f);
    }

    public void CheckAllButtonsAndActivateGoThrought()
    {
        bool allDisabled = true;
        foreach (Transform child in displayPlayerManager)
        {
            Button btn = child.GetComponent<Button>();
            if (btn != null && btn.interactable)
            {
                allDisabled = false;
                break;
            }
        }

        btnGoThrought.interactable = allDisabled;
    }

    public void SetDecreaseMove(int value)
    {
        countNotMovePlayer -= value;
    }

    public void SetInreaseMove(int value)
    {
        countNotMovePlayer += value;
    }

    public void CheckMovingPlayer()
    {
        if (countNotMovePlayer == players.Length)
        {
            ProgressSystem.Instance.CompleteProgressByType(ProgressType.ButtonGoThroughtTanpaGerak);
        }
    }
}
