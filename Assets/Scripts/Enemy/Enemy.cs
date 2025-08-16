using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float maxRangeX; // Batas gerak X untuk enemy
    public float minRangeX; // Batas gerak X untuk enemy
    public float maxRangeSodor; // Batas gerak X untuk enemy
    public float minRangeSodor; // Batas gerak X untuk enemy
    public bool isType; // Tambahkan ini

    private DrawPathMovement[] players;
    private Animator anim;

    private GameManager gameManager;
    private CovenantManager covenantManager;
    private DrawPathMovement nearestPlayer;

    private bool canMove = true;

    private Vector3 lastPosition;

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

        if (covenantManager == null)
        {
            covenantManager = FindAnyObjectByType<CovenantManager>();
        }

        if (anim == null)
        {
            anim = transform.GetComponentInChildren<Animator>();
        }
    }

    void Start()
    {
        // Jika moveSpeed >= 3, hanya atur speed animasi Move
        if (moveSpeed >= 3f && moveSpeed <= 4f)
        {
            anim.SetFloat("MoveSpeedAnim", 0.6f);
        }
        else if (moveSpeed > 4f)
        {
            anim.SetFloat("MoveSpeedAnim", 0.7f);
        }
        else
        {
            anim.SetFloat("MoveSpeedAnim", 0.5f);
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

        ControllerAnimation();
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

    private void ControllerAnimation()
    {
        if (!canMove || nearestPlayer == null) return;

        float movementThreshold = 0.0001f;
        float movementDelta = Vector3.Distance(transform.position, lastPosition);
        bool isMoving = movementDelta > movementThreshold;

        anim.SetBool("Moving", isMoving);

        if (isMoving)
        {
            MoveDirection dir;

            if (isType) // mode Sodor (gerak di Z)
            {
                dir = (transform.position.z - lastPosition.z) > 0 ? MoveDirection.Positive : MoveDirection.Negative;
            }
            else // mode Gobak (gerak di X)
            {
                dir = (transform.position.x - lastPosition.x) > 0 ? MoveDirection.Positive : MoveDirection.Negative;
            }

            anim.SetFloat("Move", (float)dir);

            // ✅ Update rotasi hanya saat bergerak
            UpdateRotationToPlayer();
        }

        lastPosition = transform.position;
    }

    private void UpdateRotationToPlayer()
    {
        if (nearestPlayer == null || anim == null) return;

        float threshold = 0.1f;

        // Ambil arah animasi sebelumnya
        float currentMove = anim.GetFloat("Move");
        float newMove = currentMove;

        if (isType)
        {
            if (nearestPlayer.transform.position.x < transform.position.x - threshold)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (nearestPlayer.transform.position.x > transform.position.x + threshold)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                newMove = FlipMove(currentMove);
            }
        }
        else
        {
            if (nearestPlayer.transform.position.z > transform.position.z + threshold)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (nearestPlayer.transform.position.z < transform.position.z - threshold)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                newMove = FlipMove(currentMove);
            }
        }

        // Terapkan perubahan arah animasi jika berbeda
        if (newMove != currentMove)
        {
            anim.SetFloat("Move", newMove);
        }
    }

    private float FlipMove(float value)
    {
        return value == 0f ? 1f : 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit " + other.name);
            anim.SetBool("Attack", true);
            gameManager.UpdateDefenderCatch(1);
            StartCoroutine(ResetAttackAnim());

            DrawPathMovement _anim = other.GetComponent<DrawPathMovement>();
            if (_anim != null)
            {
                _anim.SetCatch(true);
            }

            covenantManager.SetCovenant(1);
        }
    }

    private IEnumerator ResetAttackAnim()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("Attack", false);
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }   
}

public enum MoveDirection
{
    Positive = 0, // ke kanan / maju
    Negative = 1  // ke kiri / mundur
}

