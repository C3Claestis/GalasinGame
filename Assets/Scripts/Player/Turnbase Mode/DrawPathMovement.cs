using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.EventSystems; // Tambahkan di atas

[RequireComponent(typeof(LineRenderer))]
public class DrawPathMovement : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] DrawPathMovement[] players;
    [SerializeField] Color selectedColor = Color.white;

    private bool isSelected = false;
    private bool isDrawing = false;
    private bool canMove = false;

    private Camera mainCamera;
    private List<Vector3> pathPoints = new List<Vector3>();
    private LineRenderer lineRenderer;
    private int currentPointIndex = 0;

    private Rigidbody rb;
    private Vector3 startPoint;
    private Quaternion startRotation;
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
    }

    void Update()
    {
        HandleMouseInput();

        // Karakter hanya bergerak jika selesai menggambar dan diperbolehkan
        if (canMove)
            MoveAlongPath();

        // Freeze posisi Y jika x di luar batas
        if (transform.position.x > 12f || transform.position.x < -12f)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;

        }
        else
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
        }
    }

    void HandleMouseInput()
    {
        // Cegah raycast jika pointer di atas UI
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
                    canMove = false; // Jangan gerak saat menggambar
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
                Vector3 point = new Vector3(hit.point.x, 0.5f, hit.point.z); // Y tetap 1.25
                if (pathPoints.Count == 0 || Vector3.Distance(point, pathPoints[pathPoints.Count - 1]) > 0.05f)
                {
                    pathPoints.Add(point);
                    lineRenderer.positionCount = pathPoints.Count;
                    lineRenderer.SetPosition(pathPoints.Count - 1, point);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isSelected && isDrawing)
        {
            isDrawing = false;
            currentPointIndex = 0;
            isSelected = false; // Tidak lagi terpilih setelah selesai menggambar
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

        Vector3 target = pathPoints[currentPointIndex];
        target.y = transform.position.y;

        // Gerakkan karakter menuju titik berikutnya
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        anim.SetBool("Moving", true);
        ProgressSystem.Instance.CompleteProgressByType(ProgressType.BerhasilMenggerakanPlayer);

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
}
