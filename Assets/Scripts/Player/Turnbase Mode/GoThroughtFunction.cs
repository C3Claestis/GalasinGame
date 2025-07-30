using UnityEngine;
using Unity.Cinemachine;

public class GoThroughtFunction : MonoBehaviour
{
    [SerializeField] private DrawPathMovement[] players;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
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

        cinemachineCamera.Target.TrackingTarget = transform;
    }
}
