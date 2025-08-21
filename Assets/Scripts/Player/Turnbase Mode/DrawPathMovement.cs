using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.EventSystems; // Tambahkan di atas

[RequireComponent(typeof(LineRenderer))]
public class DrawPathMovement : MonoBehaviour
{
    [SerializeField] DrawPathMovement[] players;
    [SerializeField] Color selectedColor = Color.white;

    private bool isSelected = false;
    private bool isDrawing = false;
    private bool canMove = false;

    private CinemachineCamera cinemachineCamera;
    private Camera mainCamera;
    private List<Vector3> pathPoints = new List<Vector3>();
    private LineRenderer lineRenderer;
    private int currentPointIndex = 0;

    private Animator anim;
    private Rigidbody rb;
    private Vector3 startPoint;
    private Quaternion startRotation;

    private GoThroughtFunction goThrought;
    private bool hasReportedMove = false;

    private float firstXpos;
    private bool SingleLine = true;

    private float moveSpeed;
    private bool isMove = false;

    float minSpacing = 0.05f; // jarak minimum antar titik
    float maxSpacing = 0.1f;  // jarak maksimum antar titik
    
    void Awake()
    {
        if (cinemachineCamera == null) cinemachineCamera = GameObject.Find("Forward Camera").GetComponent<CinemachineCamera>();

        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None)
                .Where(p => p != this)
                .ToArray();
        }

        if (anim == null)
        {
            anim = transform.GetComponentInChildren<Animator>();
        }

        switch (PlayerPrefs.GetInt("SpeedLevel", 0))
        {
            case 0: moveSpeed = 3f; break;
            case 1: moveSpeed = 4f; break;
            case 2: moveSpeed = 5f; break;
            case 3: moveSpeed = 6f; break;
            case 4: moveSpeed = 8f; break;
            default: moveSpeed = 3f; break; // default kecepatan
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        if (mainCamera == null)
            mainCamera = Camera.main;

        startPoint = transform.position;
        startRotation = transform.rotation;

        if (goThrought == null)
        {
            goThrought = FindFirstObjectByType<GoThroughtFunction>();
        }

        firstXpos = transform.position.x;
    }

    void Update()
    {
        HandleMouseInput();

        // Karakter hanya bergerak jika selesai menggambar dan diperbolehkan
        if (canMove)
            MoveAlongPath();
    }

    

    void HandleMouseInput()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform) // klik karakter
                {
                    pathPoints.Clear();
                    lineRenderer.positionCount = 0;
                }
                else if (isSelected)
                {
                    isDrawing = true;
                    canMove = false;
                    pathPoints.Clear();
                    lineRenderer.positionCount = 0;
                }
            }
        }

        if (Input.GetMouseButton(0) && isSelected && isDrawing)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 point = new Vector3(hit.point.x, 0.5f, hit.point.z);

                // 🔽 Tentukan batas maksimal jumlah titik dari PlayerPrefs
                int pathLineLevel = PlayerPrefs.GetInt("PathLineLevel", 0);
                int maxPoints = 100;
                switch (pathLineLevel)
                {
                    case 1: maxPoints = 120; break;
                    case 2: maxPoints = 150; break;
                    case 3: maxPoints = 170; break;
                    case 4: maxPoints = 200; break;
                    case 5: maxPoints = 300; break;
                    default: maxPoints = 100; break;
                }

                if (pathPoints.Count == 0)
                {
                    pathPoints.Add(point);
                    lineRenderer.positionCount = 1;
                    lineRenderer.SetPosition(0, point);
                }
                else if (pathPoints.Count < maxPoints)
                {
                    Vector3 lastPoint = pathPoints[pathPoints.Count - 1];
                    float dist = Vector3.Distance(lastPoint, point);

                    if (dist >= minSpacing)
                    {
                        // Kalau lebih jauh dari maxSpacing, tambahkan beberapa titik di antaranya
                        if (dist > maxSpacing)
                        {
                            int steps = Mathf.FloorToInt(dist / maxSpacing);
                            Vector3 dir = (point - lastPoint).normalized;

                            for (int i = 1; i <= steps; i++)
                            {
                                if (pathPoints.Count >= maxPoints) break;

                                Vector3 newPoint = lastPoint + dir * (i * maxSpacing);
                                pathPoints.Add(newPoint);
                                lineRenderer.positionCount = pathPoints.Count;
                                lineRenderer.SetPosition(pathPoints.Count - 1, newPoint);
                            }
                        }
                        else
                        {
                            pathPoints.Add(point);
                            lineRenderer.positionCount = pathPoints.Count;
                            lineRenderer.SetPosition(pathPoints.Count - 1, point);
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isSelected && isDrawing)
        {
            isDrawing = false;
            currentPointIndex = 0;
            isSelected = false;
            ProgressSystem.Instance.CompleteProgressByType(ProgressType.BerhasilMenggambarLine);
        }
    }

    void MoveAlongPath()
    {
        if (pathPoints.Count == 0 || currentPointIndex >= pathPoints.Count)
            return;

        // Hitung total panjang path
        float totalLength = 0f;
        for (int i = 1; i < pathPoints.Count; i++)
        {
            totalLength += Vector3.Distance(pathPoints[i - 1], pathPoints[i]);
        }

        // Jika path terlalu pendek (< 0.05), jangan bergerak
        if (totalLength < 0.05f)
        {
            pathPoints.Clear();
            lineRenderer.positionCount = 0;
            canMove = false;
            isDrawing = false;
            isSelected = false;
            return;
        }

        // ⬇️ Panggil SetMove(-1) hanya sekali saat mulai bergerak
        if (!hasReportedMove)
        {
            goThrought.SetDecreaseMove(1); // atau goThrought.SetMove(-1) kalau hanya mengurangi
            hasReportedMove = true;
        }

        Vector3 target = pathPoints[currentPointIndex];
        target.y = transform.position.y;

        // Gerakkan karakter menuju titik berikutnya
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        anim.SetBool("Moving", true);

        ProgressSystem.Instance.CompleteProgressByType(ProgressType.BerhasilMenggerakanPlayer);

        //Check Single Line
        CheckPositionX();

        // Rotasi karakter menghadap arah gerak dengan mulus
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // 10f = kecepatan rotasi
        }

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPointIndex++;

            // Sudah sampai di titik terakhir
            if (currentPointIndex >= pathPoints.Count)
            {
                pathPoints.Clear();
                lineRenderer.positionCount = 0;
                canMove = false;
                isDrawing = false;
                isSelected = false;
                anim.SetBool("Moving", false);

                // ⬇️ Panggil SetMove(+1) saat selesai bergerak
                goThrought.SetInreaseMove(1); // atau goThrought.SetMove(+1) kalau hanya menambah
                hasReportedMove = false; // Reset agar bisa lapor lagi di perjalanan berikutnya
            }
        }
    }

    public void ResetPlayer()
    {
        // Pindahkan object ke titik awal
        transform.position = startPoint;
        gameObject.name = "Player";
        pathPoints.Clear();
        lineRenderer.positionCount = 0;
        currentPointIndex = 0;
        isDrawing = false;
        isSelected = false;
        canMove = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        anim.SetBool("Catch", false);
        anim.SetBool("Moving", false);
        transform.rotation = startRotation;
    }

    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
        if (isSelected)
        {
            cinemachineCamera.Target.TrackingTarget = transform;
        }
    }

    public Color GetColor()
    {
        return selectedColor;
    }

    public void SetIsDrawing(bool drawing) { isDrawing = drawing; }

    public void SetCanMove(bool selected) { canMove = selected; }
    public bool GetCanMove() { return canMove; }

    public bool SetCatch(bool catchState)
    {
        if (anim == null) return false;
        anim.SetBool("Catch", catchState);
        return true;
    }

    void CheckPositionX()
    {
        if (firstXpos > 0)
        {
            if (transform.position.x < 0)
            {
                SingleLine = false;
            }
        }
        else
        {
            if (transform.position.x > 0)
            {
                SingleLine = false;
            }
        }
    }

    public bool GetSingleLine() => SingleLine;
    public bool GetMoving()
    {
        if (anim == null) return false;

        // ambil parameter bool "Moving" langsung dari Animator
        isMove = anim.GetBool("Moving");

        return isMove;
    }
}
