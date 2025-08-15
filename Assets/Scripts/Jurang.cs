using UnityEngine;

public class Jurang : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform objekPenyerang;
    [SerializeField] private float speedGerak = 5f;
    [SerializeField] private float stopDistance = 1f; // meter

    // Opsional: rotasi hanya sumbu Y agar menghadap target
    [SerializeField] private bool rotateToFace = true;
    [SerializeField] private float rotateSpeed = 10f;

    private Transform targetPlayer;
    private Animator targetAnimator;
    private Animator penyerangAnimator;
    private bool mulaiKejar = false;
    private bool sudahTrigger = false;

    private Vector3 startPosPenyerang;
    private Quaternion startRotPenyerang;

    private void Awake()
    {
        if (objekPenyerang != null)
        {
            startPosPenyerang = objekPenyerang.position;
            startRotPenyerang = objekPenyerang.rotation;

            // Ambil animator penyerang (bisa dari dirinya sendiri atau child)
            penyerangAnimator = objekPenyerang.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetPlayer = other.transform;

            targetAnimator = targetPlayer.GetComponentInChildren<Animator>();

            // Mulai animasi penyerang
            if (penyerangAnimator != null)
                penyerangAnimator.SetBool("Run", true);

            mulaiKejar = true;
            sudahTrigger = false; // siap trigger saat sampai
        }
    }

    private void Update()
    {
        if (!mulaiKejar || objekPenyerang == null || targetPlayer == null) return;

        // --- Jarak planar (XZ), abaikan Y ---
        Vector2 deltaXZ = new Vector2(
            targetPlayer.position.x - objekPenyerang.position.x,
            targetPlayer.position.z - objekPenyerang.position.z
        );

        // Jika sudah dalam jarak < stopDistance, hentikan & trigger sekali
        if (deltaXZ.sqrMagnitude <= stopDistance * stopDistance)
        {
            if (!sudahTrigger)
            {
                sudahTrigger = true;
                mulaiKejar = false; // berhenti mengikuti

                if (gameManager != null)
                    gameManager.UpdateDefenderCatch(1);

                if (ProgressSystem.Instance != null)
                    ProgressSystem.Instance.CompleteProgressByType(ProgressType.KeluarAreaPertandingan);

                // Set animator catch
                if (targetAnimator != null)
                    targetAnimator.SetBool("Catch", true);

                // Mulai animasi penyerang
                if (penyerangAnimator != null)
                    penyerangAnimator.SetBool("Run", false);

                // Mulai animasi penyerang
                if (penyerangAnimator != null)
                    penyerangAnimator.SetBool("Attack", true);
            }
            return;
        }

        // --- Gerak hanya pada XZ: target Y = Y penyerang saat ini ---
        Vector3 targetPosXZ = new Vector3(
            targetPlayer.position.x,
            objekPenyerang.position.y, // kunci Y
            targetPlayer.position.z
        );

        objekPenyerang.position = Vector3.MoveTowards(
            objekPenyerang.position,
            targetPosXZ,
            speedGerak * Time.deltaTime
        );

        // Opsional: rotasi ke arah target pada sumbu Y saja
        if (rotateToFace)
        {
            Vector3 lookDir = targetPosXZ - objekPenyerang.position;
            lookDir.y = 0f; // hanya Y-rotation
            if (lookDir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                objekPenyerang.rotation = Quaternion.Slerp(
                    objekPenyerang.rotation, targetRot, rotateSpeed * Time.deltaTime
                );
            }
        }
    }

    // 3) Fungsi untuk tombol di Inspector: kembalikan penyerang ke posisi awal
    public void KembalikanObjekPenyerang()
    {
        if (objekPenyerang == null) return;

        objekPenyerang.position = startPosPenyerang;
        objekPenyerang.rotation = startRotPenyerang;

        // Mulai animasi penyerang
        if (penyerangAnimator != null)
            penyerangAnimator.SetBool("Attack", false);

        mulaiKejar = false;
        sudahTrigger = false;
        targetPlayer = null;
    }
}
