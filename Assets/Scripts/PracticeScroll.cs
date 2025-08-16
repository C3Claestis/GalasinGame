using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PracticeScroll : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private ScrollRect scrollRect;  // Referensi ScrollRect
    [SerializeField] private float stepTime = 0.25f; // Waktu animasi transisi

    private int totalItems;
    private int currentIndex = 0;
    private bool isScrolling = false;
    private RectTransform content;

    private void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();

        content = scrollRect.content;
        totalItems = content.childCount;  // hitung otomatis jumlah item
        SetScrollPosition(0); // mulai dari awal
    }

    public void Next()
    {
        if (isScrolling) return;
        if (currentIndex < totalItems - 1)
        {
            currentIndex++;
            MoveToIndex(currentIndex);
        }
    }

    public void Prev()
    {
        if (isScrolling) return;
        if (currentIndex > 0)
        {
            currentIndex--;
            MoveToIndex(currentIndex);
        }
    }

    private void MoveToIndex(int index)
    {
        float targetPos = (float)index / (totalItems - 1);
        StartCoroutine(SmoothScroll(targetPos));
    }

    private IEnumerator SmoothScroll(float target)
    {
        isScrolling = true;
        float start = scrollRect.horizontalNormalizedPosition;
        float elapsed = 0f;

        while (elapsed < stepTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / stepTime);
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, target, t);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = target;
        isScrolling = false;
    }

    private void SetScrollPosition(int index)
    {
        currentIndex = index;
        scrollRect.horizontalNormalizedPosition = (float)index / (totalItems - 1);
    }
}
