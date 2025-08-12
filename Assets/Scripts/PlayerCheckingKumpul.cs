using UnityEngine;

public class PlayerCheckingKumpul : MonoBehaviour
{
    private DrawPathMovement[] players;

    [SerializeField] private float minZ = -6f;
    [SerializeField] private float maxZ = 6f;

    bool isInitialized = true;

    void Start()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }
    }
    private void Update()
    {
        if (players == null || players.Length == 0) return;

        if (isInitialized)
        {
            // Cek semua di X negatif
            bool semuaDiXNegatif = true;
            bool semuaDalamRangeZ_XNegatif = true;

            // Cek semua di X positif
            bool semuaDiXPositif = true;
            bool semuaDalamRangeZ_XPositif = true;

            foreach (var player in players)
            {
                if (player == null) continue;
                Vector3 pos = player.transform.position;

                // Untuk grup X negatif
                if (pos.x >= 0) semuaDiXNegatif = false;
                if (pos.z < minZ || pos.z > maxZ) semuaDalamRangeZ_XNegatif = false;

                // Untuk grup X positif
                if (pos.x <= 0) semuaDiXPositif = false;
                if (pos.z < minZ || pos.z > maxZ) semuaDalamRangeZ_XPositif = false;
            }

            // Trigger log untuk masing-masing sisi
            if (semuaDiXNegatif && semuaDalamRangeZ_XNegatif)
            {
                ProgressSystem.Instance.CompleteProgressByType(ProgressType.BerkumpulTengahSisiKiri);
                isInitialized = false; // Hentikan pengecekan setelah trigger
            }
            if (semuaDiXPositif && semuaDalamRangeZ_XPositif)
            {
                ProgressSystem.Instance.CompleteProgressByType(ProgressType.BerkumpulTengahSisiKanan);
                isInitialized = false; // Hentikan pengecekan setelah trigger
            }
        }
    }
}
