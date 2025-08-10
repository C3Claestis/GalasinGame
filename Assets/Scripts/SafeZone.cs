using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] bool isUjung;
    [SerializeField] CovenantManager covenantManager;
  
    void OnTriggerEnter(Collider other)
    {
        // Jika masuk ke ujung dan sudah sampai ujung
        if (other.CompareTag("Player") && other.name == "Warrior" && isUjung)
        {
            other.name = "Gladiator";
        }

        //Jika masuk ke base dan sudah sampai base setelah dari ujung
        if (other.CompareTag("Player") && other.name == "Conqueror" && !isUjung)
        {
            other.name = "Champion";
            covenantManager.CheckPlayerFinished();
        }

    }
    void OnTriggerExit(Collider other)
    {
        //Jika keluar dari base dan mulai ke ujung
        if (other.CompareTag("Player") && other.name != "Warrior" && !isUjung)
        {
            other.name = "Warrior";
        }

        // Jika sudah sampai ujung dan mulai kembali ke base
        if (other.CompareTag("Player") && other.name == "Gladiator" && isUjung)
        {
            other.name = "Conqueror";
        }
    }
}
