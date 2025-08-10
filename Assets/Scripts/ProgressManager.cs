using UnityEngine;
using System.ComponentModel;

[CreateAssetMenu(fileName = "Progress Manager", menuName = "ScriptableObjects/ProgressManager")]
public class ProgressManager : ScriptableObject
{
    public string title;

    [Header("Progress Conditions (enum = label)")]
    public ProgressType progress1;
    public ProgressType progress2;
    public ProgressType progress3;

    [Header("Progress System Indices (auto filled)")]
    [HideInInspector] public int progress1Index;
    [HideInInspector] public int progress2Index;
    [HideInInspector] public int progress3Index;

    [Header("Runtime state (will be set at runtime)")]
    public bool isProgress1Complete;
    public bool isProgress2Complete;
    public bool isProgress3Complete;

    public delegate void StarUpdatedHandler(int starCount);
    public static event StarUpdatedHandler OnStarUpdated;

    private void OnEnable()
    {
        // Auto isi index dari enum
        progress1Index = (int)progress1;
        progress2Index = (int)progress2;
        progress3Index = (int)progress3;

        ProgressSystem.OnProgressCompletedIndex += CheckProgressByIndex;
    }

    private void OnDisable()
    {
        ProgressSystem.OnProgressCompletedIndex -= CheckProgressByIndex;
    }

    private void CheckProgressByIndex(int index)
    {
        Debug.Log($"[ProgressManager] Received index: {index}");

        if (index == progress1Index)
        {
            isProgress1Complete = true;
            Debug.Log("Progress 1 complete!");
        }
        else if (index == progress2Index)
        {
            isProgress2Complete = true;
            Debug.Log("Progress 2 complete!");
        }
        else if (index == progress3Index)
        {
            isProgress3Complete = true;
            Debug.Log("Progress 3 complete!");
        }
        else
        {
            Debug.LogWarning($"Index {index} tidak cocok dengan progress1/2/3 index.");
            return; // Jangan update bintang kalau nggak cocok
        }

        OnStarUpdated?.Invoke(GetStarCount());
    }

    public int GetStarCount()
    {
        int count = 0;
        if (isProgress1Complete) count++;
        if (isProgress2Complete) count++;
        if (isProgress3Complete) count++;
        return count;
    }

    public void ResetProgress()
    {
        isProgress1Complete = false;
        isProgress2Complete = false;
        isProgress3Complete = false;
        OnStarUpdated?.Invoke(0);
    }

    // Ambil teks untuk UI
    public string GetProgress1Description() => progress1.GetDescription();
    public string GetProgress2Description() => progress2.GetDescription();
    public string GetProgress3Description() => progress3.GetDescription();
}

public enum ProgressType
{
    [Description("Berhasil Menggambar Line")]
    BerhasilMenggambarLine,

    [Description("Berhasil Menggerakan Player")]
    BerhasilMenggerakanPlayer,

    [Description("Berhasil Go Throught")]
    BerhasilGoThrought,

    [Description("Berhasil menekan tombol kamera Right/Left/Center")]
    TekanKameraRightLeftCenter, // Nama aman di kode

    [Description("Berhasil Go Throught di atas 1 menit")]
    GoThrought1Menit,

    [Description("Berhasil Go Throught di atas 2 menit")]
    GoThrought2Menit,

    [Description("Berhasil Go Throught di atas 30 detik")]
    GoThrought30Detik,

    [Description("Berhasil jatuh ke jurang")]
    JatuhKeJurang,

    [Description("Berhasil menekan tombol untuk bergerak")]
    TekanTombolBergerak,

    [Description("Berhasil menekan button Go Throught tanpa menggerakan karakter")]
    ButtonGoThroughtTanpaGerak,

    [Description("Berhasil menembus semua penjaga hanya dengan bergerak di satu jalur")]
    MenembusPenjagaDenganSatuSisiSaja,

    [Description("Berhasil berkumpul dalam satu titik yang sama di tengah sisi kiri")]
    BerkumpulTengahSisiKiri,

    [Description("Berhasil berkumpul dalam satu titik yang sama di tengah sisi kanan")]
    BerkumpulTengahSisiKanan,

    [Description("Berhasil menunggu di ujung maksimal 30 detik")]
    TungguUjung30Detik,

    [Description("Berhasil menunggu di ujung maksimal 1 menit")]
    TungguUjung1Menit,

    [Description("Berhasil menunggu di ujung maksimal 10 detik")]
    TungguUjung10Detik,

    [Description("Berhasil selesai sebelum 5 covenant")]
    SelesaiSebelum5Covenant,

    [Description("Berhasil selesai sebelum 7 covenant")]
    SelesaiSebelum7Covenant,  

    [Description("Berhasil selesai sebelum 3 covenant")]
    SelesaiSebelum3Covenant,

    [Description("Berhasil selesai sebelum 1 covenant")]
    SelesaiSebelum1Covenant
}

// 0. Berhasil Menggambar Line ✅ 
// 1. Berhasil Menggerakan Player ✅
// 2. Berhasil Go Throught ✅
// 3. Berhasil menekan tombol kamera right/left/center ✅
// 4. Berhasil Go Throught di atas 1 menit  ✅
// 5. Berhasil Go Throught di atas 2 menit  ✅
// 6. Berhasil Go Throught di atas 30 detik ✅
// 7. Berhasil jatuh ke jurang ✅
// 8. Berhasil menekan tombol untuk bergerak ✅
// 9. Berhasil menekan button go throught tanpa menggerakan karakter ✅
// 10. Berhasil menembus semua penjaga hanya dengan bergerak di satu jalur ✅
// 11. Berhasil berkumpul dalam satu titik yang sama di tengah sisi kiri
// 12. Berhasil berkumpul dalam satu titik yang sama di tengah sisi kanan
// 13. Berhasil menunggu di ujung maksimal 30 detik
// 14. Berhasil menunggu di ujung maksimal 1 menit
// 15. Berhasil menunggu di ujung maksimal 10 detik
// 16. Berhasil selesai sebelum 5 covenant ✅
// 17. Berhasil selesai sebelum 7 covenant ✅
// 18. Berhasil selesai sebelum 3 covenant ✅
// 19. Berhasil selesai sebelum 1 covenant ✅
