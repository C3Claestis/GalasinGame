using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.Cinemachine;

[RequireComponent(typeof(LineRenderer))]
public class DrawPathMovement : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] DrawPathMovement[] players;

    private bool isSelected = false;
    private bool isDrawing = false;
    private bool canMove = false;

    private List<Vector3> pathPoints = new List<Vector3>();
    private LineRenderer lineRenderer;
    private int currentPointIndex = 0;

    void Awake()
    {
        if (cinemachineCamera == null) cinemachineCamera = GameObject.Find("Camera Player").GetComponent<CinemachineCamera>();

        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None)
                .Where(p => p != this)
                .ToArray();
        }
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        if (mainCamera == null)
            mainCamera = Camera.main;
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

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPointIndex++;

            // Sudah sampai di titik terakhir
            if (currentPointIndex >= pathPoints.Count)
            {
                // Hapus garis
                pathPoints.Clear();
                lineRenderer.positionCount = 0;
                canMove = false;
                isDrawing = false;
                isSelected = false;
            }
        }
    }

    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
        if (isSelected)
        {
            cinemachineCamera.Target.TrackingTarget = transform;
        }
    }

    public void SetCanMove(bool selected) { canMove = selected; }
}
