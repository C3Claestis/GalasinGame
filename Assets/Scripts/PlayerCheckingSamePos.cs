using UnityEngine;

public class PlayerCheckingSamePos : MonoBehaviour
{
    private DrawPathMovement[] players;

    private float timer10;
    private float timer30;
    private float timer60;

    private bool func10Active;
    private bool func30Active;
    private bool func60Active;

    private bool progress10Done;
    private bool progress30Done;
    private bool progress60Done;

    void Start()
    {
        if (players == null || players.Length == 0)
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (IsAnyGladiator())
        {
            if (!func10Active && !func30Active && !func60Active)
                StartAllTimers();
        }

        // 10 detik
        if (func10Active && !progress10Done)
        {
            timer10 -= Time.deltaTime;
            if (timer10 <= 0f) progress10Done = true;  
        }

        // 30 detik
        if (func30Active && !progress30Done)
        {
            timer30 -= Time.deltaTime;
            if (timer30 <= 0f)  progress30Done = true;          
        }

        // 60 detik
        if (func60Active && !progress60Done)
        {
            timer60 -= Time.deltaTime;
            if (timer60 <= 0f)
            {
                progress60Done = true;           
                enabled = false;
            }
        }
    }

    public void StartAllTimers()
    {
        timer10 = 10f;
        timer30 = 30f;
        timer60 = 60f;
        func10Active = true;
        func30Active = true;
        func60Active = true;

        progress10Done = false;
        progress30Done = false;
        progress60Done = false;

        Debug.Log("▶ Semua timer dimulai: 10s, 30s, 60s");
    }

    public bool HasAnyGladiator()
    {
        foreach (var p in players)
        {
            if (p != null && p.gameObject.name == "Gladiator")
                return true;
        }
        return false;
    }

    public void OnNewGladiator()
    {
        // Kalau belum selesai, langsung eksekusi sesuai timer aktif
        if (func10Active && !progress10Done)
        {
            progress10Done = true;
            Debug.Log("⚡ Real-time: 10 detik selesai karena gladiator baru");
            ProgressSystem.Instance.CompleteProgressByType(ProgressType.TungguUjung10Detik);
        }
        if (func30Active && !progress30Done)
        {
            progress30Done = true;
            Debug.Log("⚡ Real-time: 30 detik selesai karena gladiator baru");
            ProgressSystem.Instance.CompleteProgressByType(ProgressType.TungguUjung30Detik);
        }
        if (func60Active && !progress60Done)
        {
            progress60Done = true;
            Debug.Log("⚡ Real-time: 1 menit selesai karena gladiator baru");
            ProgressSystem.Instance.CompleteProgressByType(ProgressType.TungguUjung1Menit);
            enabled = false; // matikan script setelah selesai
        }
    }

    private bool IsAnyGladiator()
    {
        foreach (var p in players)
        {
            if (p != null && p.gameObject.name == "Gladiator")
                return true;
        }
        return false;
    }
}
