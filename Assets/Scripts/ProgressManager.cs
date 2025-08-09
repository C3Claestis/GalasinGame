using UnityEngine;
using System.ComponentModel;

[CreateAssetMenu(fileName = "Progress Manager", menuName = "ScriptableObjects/ProgressManager")]
public class ProgressManager : ScriptableObject
{
    public string title;

    [Header("Progress Conditions")]
    public ProgressType progress1;
    public bool isProgress1Complete;

    public ProgressType progress2;
    public bool isProgress2Complete;

    public ProgressType progress3;
    public bool isProgress3Complete;

    // Hitung bintang berdasarkan progres
    public int GetStarCount()
    {
        int starCount = 0;
        if (isProgress1Complete) starCount++;
        if (isProgress2Complete) starCount++;
        if (isProgress3Complete) starCount++;
        return starCount;
    }

    // Reset progress
    public void ResetProgress()
    {
        isProgress1Complete = false;
        isProgress2Complete = false;
        isProgress3Complete = false;
    }

    // Ambil teks progress dengan deskripsi asli
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

    [Description("Berhasil bergerak sampai ujung dalam 30 detik")]
    SampaiUjung30Detik,

    [Description("Berhasil bergerak sampai ujung dalam 1 menit")]
    SampaiUjung1Menit,

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

    [Description("Berhasil selesai sebelum 9 covenant")]
    SelesaiSebelum9Covenant,

    [Description("Berhasil selesai sebelum 3 covenant")]
    SelesaiSebelum3Covenant,

    [Description("Berhasil selesai sebelum 1 covenant")]
    SelesaiSebelum1Covenant
}


// 1. Berhasil Menggambar Line 
// 2. Berhasil Menggerakan Player
// 3. Berhasil Go Throught 
// 4. Berhasil menekan tombol kamera right/left/center
// 5. Berhasil Go Throught di atas 1 menit
// 6. Berhasil Go Throught di atas 2 menit
// 7. Berhasil Go Throught di atas 30 detik
// 8. Berhasil jatuh ke jurang
// 9. Berhasil menekan tombol untuk bergerak
// 10. Berhasil menekan button go throught tanpa menggerakan karakter
// 11. Berhasil bergerak selama sampai ujung dalam 30 detik
// 12. Berhasil bergerak selama sampai ujung dalam 1 menit
// 13. Berhasil menunggu di ujung maksimal 30 detik
// 14. Berhasil menunggu di ujung maksimal 1 menit
// 15. Berhasil menunggu di ujung maksimal 10 detik
// 16. Berhasil selesai sebelum 5 covenant
// 17. Berhasil selesai sebelum 7 covenant
// 18. Berhasil selesai sebelum 9 covenant
// 19. Berhasil selesai sebelum 3 covenant
// 20. Berhasil selesai sebelum 1 covenant
