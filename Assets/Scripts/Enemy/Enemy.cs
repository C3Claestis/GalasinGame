using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] DrawPathMovement[] players;

    [HideInInspector] public float moveSpeed;

    [HideInInspector] public float maxRangeX; // Batas gerak X untuk enemy
    [HideInInspector] public float minRangeX; // Batas gerak X untuk enemy
    [HideInInspector] public float maxRangeSodor; // Batas gerak X untuk enemy
    [HideInInspector] public float minRangeSodor; // Batas gerak X untuk enemy

    [Header("Type")]
    [HideInInspector] public bool isType; // Tambahkan ini

    private GameManager gameManager;
    private DrawPathMovement nearestPlayer;

    private bool canMove = true;

    private void Awake()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (isType)
            {
                MoveToNearestPlayerSodor();
            }
            else
            {
                MoveToNearestPlayerX();
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

        float clampedX = Mathf.Clamp(nearestPlayer.transform.position.x, minRangeX, maxRangeX);
        Vector3 targetPos = new Vector3(clampedX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Gerak ke sumbu Z (Sodor)
    /// </summary>
    public void MoveToNearestPlayerSodor()
    {
        FindNearestPlayer();
        if (nearestPlayer == null) return;

        float clampedZ = Mathf.Clamp(nearestPlayer.transform.position.z, minRangeSodor, maxRangeSodor);
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, clampedZ);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit " + other.name);
            gameManager.UpdateDefenderCatch(1);
        }
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
