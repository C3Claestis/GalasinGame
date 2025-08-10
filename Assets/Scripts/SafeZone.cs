using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] private CovenantManager covenantManager;
    [SerializeField] private DrawPathMovement[] players;

    void Start()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }
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
            }

            // 3. Jika Gladiator dan < 12 → jadi Conqueror
            if (name == "Gladiator" && z < 12f)
            {
                t.name = "Conqueror";
            }

            // 4. Jika Conqueror dan z antara -13 sampai -19 → jadi Champion
            if (name == "Conqueror" && z <= -13f && z >= -19f)
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
