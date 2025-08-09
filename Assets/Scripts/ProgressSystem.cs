using UnityEngine;

public class ProgressSystem : MonoBehaviour
{
    public static ProgressSystem Instance { get; private set; }

    private bool[] progressComplete = new bool[20];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // kalau mau persist antar scene
    }

    /// <summary>
    /// Menandai progress sesuai index (0-based)
    /// </summary>
    private void CompleteProgress(int index)
    {
        if (index < 0 || index >= progressComplete.Length) return;
        if (!progressComplete[index])
        {
            progressComplete[index] = true;
            Debug.Log($"Progress {index + 1} selesai!");
        }
    }

    public bool IsProgressComplete(int index)
    {
        if (index < 0 || index >= progressComplete.Length) return false;
        return progressComplete[index];
    }

    public int GetStarCount()
    {
        int count = 0;
        foreach (bool completed in progressComplete)
        {
            if (completed) count++;
        }
        return count;
    }

    public void ResetAllProgress()
    {
        for (int i = 0; i < progressComplete.Length; i++)
        {
            progressComplete[i] = false;
        }
    }

    #region Fungsi Progress
    public void DrawLineSuccess() => CompleteProgress(0);              // 1. Berhasil Menggambar Line
    public void MovePlayerSuccess() => CompleteProgress(1);            // 2. Berhasil Menggerakan Player
    public void GoThroughSuccess() => CompleteProgress(2);             // 3. Berhasil Go Throught
    public void CameraButtonRightLeftCenter() => CompleteProgress(3);  // 4. Berhasil menekan tombol kamera right/left/center
    public void GoThroughOver1Min() => CompleteProgress(4);            // 5. Berhasil Go Throught di atas 1 menit
    public void GoThroughOver2Min() => CompleteProgress(5);            // 6. Berhasil Go Throught di atas 2 menit
    public void GoThroughOver30Sec() => CompleteProgress(6);           // 7. Berhasil Go Throught di atas 30 detik
    public void FallIntoPit() => CompleteProgress(7);                  // 8. Berhasil jatuh ke jurang
    public void PressMoveButton() => CompleteProgress(8);              // 9. Berhasil menekan tombol untuk bergerak
    public void GoThroughWithoutMove() => CompleteProgress(9);         // 10. Berhasil menekan button go throught tanpa menggerakan karakter
    public void ReachEndUnder30Sec() => CompleteProgress(10);          // 11. Berhasil bergerak sampai ujung dalam 30 detik
    public void ReachEndUnder1Min() => CompleteProgress(11);           // 12. Berhasil bergerak sampai ujung dalam 1 menit
    public void WaitAtEndMax30Sec() => CompleteProgress(12);           // 13. Berhasil menunggu di ujung maksimal 30 detik
    public void WaitAtEndMax1Min() => CompleteProgress(13);            // 14. Berhasil menunggu di ujung maksimal 1 menit
    public void WaitAtEndMax10Sec() => CompleteProgress(14);           // 15. Berhasil menunggu di ujung maksimal 10 detik
    public void FinishBefore5Covenant() => CompleteProgress(15);       // 16. Berhasil selesai sebelum 5 covenant
    public void FinishBefore7Covenant() => CompleteProgress(16);       // 17. Berhasil selesai sebelum 7 covenant
    public void FinishBefore9Covenant() => CompleteProgress(17);       // 18. Berhasil selesai sebelum 9 covenant
    public void FinishBefore3Covenant() => CompleteProgress(18);       // 19. Berhasil selesai sebelum 3 covenant
    public void FinishBefore1Covenant() => CompleteProgress(19);       // 20. Berhasil selesai sebelum 1 covenant
    #endregion
}
