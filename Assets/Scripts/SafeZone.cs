using UnityEngine;

public class SafeZone : MonoBehaviour
{

    [SerializeField] private CovenantManager covenantManager;
    [SerializeField] private DrawPathMovement[] players;

    [SerializeField] float awalSafeZone;
    [SerializeField] float akhirSafeZone;

    [SerializeField] float ujungSafeZone;

    private PlayerCheckingSamePos checker;
    public float GetUjungSafeZone()
    {
        return ujungSafeZone;
    }

    void Start()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        checker = GetComponent<PlayerCheckingSamePos>();
    }

    void Update()
    {
        foreach (var p in players)
        {
            if (p == null) continue;
            Transform t = p.transform;

            // Pastikan objeknya punya tag Player
            if (!t.CompareTag("Player")) continue;

            string name = t.name;
            float z = t.position.z;

            // 1. Jika sudah kurang dari -12 dan tag player → jadi Warrior
            if (z < -12f && name != "Warrior" && name != "Gladiator" && name != "Conqueror" && name != "Champion")
            {
                t.name = "Warrior";
            }

            // 2. Jika >= 12 dan tag Warrior → jadi Gladiator
            if (z >= 12f && name == "Warrior")
            {
                t.name = "Gladiator";

                // Kalau belum ada gladiator sebelumnya → mulai timer
                if (!checker.HasAnyGladiator())
                {
                    checker.StartAllTimers();
                }
                else
                {
                    // Kalau gladiator sudah ada, berarti ini gladiator baru → langsung eksekusi progress real-time
                    checker.OnNewGladiator();
                }
            }

            // 3. Jika Gladiator dan < 12 → jadi Conqueror
            if (name == "Gladiator" && z < ujungSafeZone)
            {
                t.name = "Conqueror";
            }

            // 4. Jika Conqueror dan z antara -13 sampai -19 → jadi Champion
            if (name == "Conqueror" && z <= awalSafeZone && z >= akhirSafeZone)
            {
                t.name = "Champion";
                covenantManager.CheckPlayerFinished();

                if (p.GetSingleLine())
                {
                    ProgressSystem.Instance.CompleteProgressByType(
                        ProgressType.MenembusPenjagaDenganSatuSisiSaja
                    );
                }
            }
        }
    }
}
