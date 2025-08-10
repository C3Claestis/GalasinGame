using UnityEngine;

public class Jurang : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tampilkan panel retake
            gameManager.UpdateDefenderCatch(1);

            ProgressSystem.Instance.CompleteProgressByType(ProgressType.JatuhKeJurang);       
        }
    }
}
