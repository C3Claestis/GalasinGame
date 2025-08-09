using UnityEngine;

public class ProgressSystem : MonoBehaviour
{
    public static ProgressSystem Instance { get; private set; }

    // 20 progress
    public bool[] progressComplete = new bool[20];

    // Event yang mengirim index (int)
    public delegate void ProgressCompletedIndexHandler(int index);
    public static event ProgressCompletedIndexHandler OnProgressCompletedIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Panggil dari Button OnClick lewat inspector: CompleteProgressByIndex(int)
    public void CompleteProgressByIndex(int index)
    {
        if (index < 0 || index >= progressComplete.Length) return;

        if (!progressComplete[index])
        {
            progressComplete[index] = true;
            Debug.Log($"ProgressSystem: index {index} completed.");
            OnProgressCompletedIndex?.Invoke(index);
        }
    }

    // Optional: panggil berdasarkan enum (casting ke int)
    public void CompleteProgressByType(ProgressType type)
    {
        CompleteProgressByIndex((int)type);
    }

    public void ResetAllProgress()
    {
        for (int i = 0; i < progressComplete.Length; i++)
            progressComplete[i] = false;
    }
}
