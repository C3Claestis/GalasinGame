using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] DrawPathMovement[] players;
    [SerializeField] float moveSpeed = 3f;
    private DrawPathMovement nearestPlayer;
    private bool isEnemyTurn = false;

    private void Awake()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemyTurn)
        {
            MoveToNearestPlayerX();
            // Cek jika sudah sampai target X, matikan giliran
            if (nearestPlayer != null && Mathf.Abs(transform.position.x - nearestPlayer.transform.position.x) < 0.01f)
            {
                isEnemyTurn = false;                
            }
        }
    }

    /// <summary>
    /// Panggil fungsi ini saat giliran Enemy bergerak di turn base.
    /// </summary>
    public void MoveToNearestPlayerX()
    {
        FindNearestPlayer();
        if (nearestPlayer == null) return;

        float clampedX = Mathf.Clamp(nearestPlayer.transform.position.x, -11.5f, 11.5f);
        Vector3 targetPos = new Vector3(clampedX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    public void MoveToRandomX()
    {
        float randomX = Random.Range(-11.5f, 11.5f); // Ganti range sesuai kebutuhan
        Vector3 targetPos = new Vector3(randomX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void FindNearestPlayer()
    {
        float minDist = float.MaxValue;
        nearestPlayer = null;

        foreach (var player in players)
        {
            if (player == null) continue;
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestPlayer = player;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (players == null) return;

        // Garis ke player terdekat (merah)
        if (nearestPlayer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, nearestPlayer.transform.position);
        }
    }

    // Fungsi untuk mengaktifkan giliran Enemy dari turn manager
    public void SetStartEnemyTurn(bool value)
    {
        FindNearestPlayer();
        isEnemyTurn = value;
    }
}
