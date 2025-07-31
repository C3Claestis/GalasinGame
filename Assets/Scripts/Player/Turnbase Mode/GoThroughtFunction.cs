using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class GoThroughtFunction : MonoBehaviour
{    
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform displayPlayerManager;
    [SerializeField] private GameObject btnGoThrought;
    [SerializeField] private DrawPathMovement[] players;    
    [SerializeField] private Enemy[] enemies;

    private void Awake()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        if (enemies == null || enemies.Length == 0)
        {
            enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        }

        if (displayPlayerManager == null)
        {
            displayPlayerManager = GameObject.Find("DisplayPlayer").GetComponent<Transform>();
        }
    }
    
    public void GoThroughAllPlayers()
    {
        foreach (var player in players)
        {
            if (player != null)
            {
                player.SetCanMove(true);
            }
        }

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetStartEnemyTurn(true);
            }
        }

        cinemachineCamera.Target.TrackingTarget = transform;
    }

    public void CheckAllButtonsAndActivateGoThrought()
    {
        bool allInactive = true;
        foreach (Transform child in displayPlayerManager)
        {
            Button btn = child.GetComponent<Button>();
            if (btn != null && btn.interactable)
            {
                allInactive = false;
                break;
            }
        }

        btnGoThrought.SetActive(allInactive);
    }
}
