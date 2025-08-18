using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("Window Skill")]
    [SerializeField] Button kamuflaseSkillWindow;
    [SerializeField] Button outAreaSkillWindow;
    [SerializeField] Button decreaseCovenantSkillWindow;

    [Header("Cooldown")]
    [SerializeField] Image kamuflaseCooldownImage;
    [SerializeField] Image outAreaCooldownImage;
    [SerializeField] Image decreaseCovenantCooldownImage;

    private float kamuflaseCooldown;
    private float outAreaCooldown;
    private float decreaseCovenantCooldown;

    [Header("Component")]
    private DrawPathMovement[] players;
    private GameObject[] vfxObjectsKamuflase;
    private GameObject[] vfxObjectsOutArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("KamuflaseLevel", 5);
        PlayerPrefs.SetInt("OutAreaLevel", 5);
        PlayerPrefs.SetInt("DecreaseCovenantLevel", 1);

        if (PlayerPrefs.GetInt("KamuflaseLevel") != 0)
        {
            kamuflaseSkillWindow.gameObject.SetActive(true);
        }
        else { kamuflaseSkillWindow.gameObject.SetActive(false); }

        if (PlayerPrefs.GetInt("OutAreaLevel") != 0)
        {
            outAreaSkillWindow.gameObject.SetActive(true);
        }
        else { outAreaSkillWindow.gameObject.SetActive(false); }

        if (PlayerPrefs.GetInt("DecreaseCovenantLevel") != 0)
        {
            decreaseCovenantSkillWindow.gameObject.SetActive(true);
        }
        else { decreaseCovenantSkillWindow.gameObject.SetActive(false); }

        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        // ✅ inisialisasi array vfx dengan panjang sama dengan jumlah player
        vfxObjectsKamuflase = new GameObject[players.Length];
        vfxObjectsOutArea = new GameObject[players.Length];

        for (int i = 0; i < players.Length; i++) // cek kalau child ke-2 ada (hindari error kalau child kurang)
        {
            if (players[i].transform.childCount > 2)
            {
                vfxObjectsKamuflase[i] = players[i].transform.GetChild(1).gameObject;
                vfxObjectsOutArea[i] = players[i].transform.GetChild(2).gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Skill Kamuflase
    public void ActiveKamuflaseSkill()
    {
        int level = PlayerPrefs.GetInt("KamuflaseLevel");

        if (level > 0)
        {
            float duration = 1f; // default
            kamuflaseCooldown = 60f; // default

            switch (level)
            {
                case 1:
                    duration = 1f;
                    kamuflaseCooldown = 60f;
                    break;
                case 2:
                    duration = 1.2f;
                    kamuflaseCooldown = 40f;
                    break;
                case 3:
                    duration = 1.5f;
                    kamuflaseCooldown = 30f;
                    break;
                case 4:
                    duration = 1.7f;
                    kamuflaseCooldown = 20f;
                    break;
                case 5:
                    duration = 2f;
                    kamuflaseCooldown = 15f;
                    break;
            }

            // Jalankan efek skill + cooldown UI
            StartCoroutine(Kamuflase(duration));
            StartCoroutine(KamuflaseCooldownRoutine(kamuflaseCooldown));
        }
    }

    IEnumerator Kamuflase(float timer)
    {
        // aktifkan kamuflase: ubah tag + nyalakan vfx
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.tag = "Kamuflase";

            if (vfxObjectsKamuflase != null && i < vfxObjectsKamuflase.Length && vfxObjectsKamuflase[i] != null)
            {
                vfxObjectsKamuflase[i].SetActive(true);
            }
        }

        yield return new WaitForSeconds(timer);

        // nonaktifkan kamuflase: kembalikan tag + matikan vfx
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.tag = "Player";

            if (vfxObjectsKamuflase != null && i < vfxObjectsKamuflase.Length && vfxObjectsKamuflase[i] != null)
            {
                vfxObjectsKamuflase[i].SetActive(false);
            }
        }
    }

    IEnumerator KamuflaseCooldownRoutine(float cooldownTime)
    {
        // aktifkan image cooldown
        kamuflaseCooldownImage.gameObject.SetActive(true);
        kamuflaseCooldownImage.fillAmount = 1f;

        // disable tombol
        if (kamuflaseSkillWindow != null)
            kamuflaseSkillWindow.interactable = false;

        float timer = 0f;
        while (timer < cooldownTime)
        {
            timer += Time.deltaTime;
            kamuflaseCooldownImage.fillAmount = 1f - (timer / cooldownTime);
            yield return null;
        }

        // selesai cooldown
        kamuflaseCooldownImage.gameObject.SetActive(false);
        if (kamuflaseSkillWindow != null)
            kamuflaseSkillWindow.interactable = true;
    }
    #endregion

    #region Skill Out Area
    public void ActiveOutAreaSkill()
    {
        int level = PlayerPrefs.GetInt("OutAreaLevel");

        if (level > 0)
        {
            float duration = 5f; // default
            outAreaCooldown = 120f; // default

            switch (level)
            {
                case 1:
                    duration = 5f;
                    outAreaCooldown = 120f;
                    break;
                case 2:
                    duration = 10f;
                    outAreaCooldown = 100f;
                    break;
                case 3:
                    duration = 15f;
                    outAreaCooldown = 80f;
                    break;
                case 4:
                    duration = 20f;
                    outAreaCooldown = 60f;
                    break;
                case 5:
                    duration = 20f;
                    outAreaCooldown = 30f;
                    break;
            }

            // Jalankan efek skill + cooldown UI
            StartCoroutine(OutArea(duration));
            StartCoroutine(OutAreaCooldownRoutine(outAreaCooldown));
        }
    }

    IEnumerator OutArea(float timer)
    {
        // aktifkan out area: ubah tag + nyalakan vfx
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.tag = "OutArea";

            if (vfxObjectsOutArea != null && i < vfxObjectsOutArea.Length && vfxObjectsOutArea[i] != null)
            {
                vfxObjectsOutArea[i].SetActive(true);
            }
        }

        yield return new WaitForSeconds(timer);

        // nonaktifkan out area: kembalikan tag + matikan vfx
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.tag = "Player";

            if (vfxObjectsOutArea != null && i < vfxObjectsOutArea.Length && vfxObjectsOutArea[i] != null)
            {
                vfxObjectsOutArea[i].SetActive(false);
            }
        }
    }

    IEnumerator OutAreaCooldownRoutine(float cooldownTime)
    {
        // aktifkan image cooldown
        outAreaCooldownImage.gameObject.SetActive(true);
        outAreaCooldownImage.fillAmount = 1f;

        // disable tombol
        if (outAreaSkillWindow != null)
            outAreaSkillWindow.interactable = false;

        float timer = 0f;
        while (timer < cooldownTime)
        {
            timer += Time.deltaTime;
            outAreaCooldownImage.fillAmount = 1f - (timer / cooldownTime);
            yield return null;
        }

        // selesai cooldown
        outAreaCooldownImage.gameObject.SetActive(false);
        if (outAreaSkillWindow != null)
            outAreaSkillWindow.interactable = true;
    }
    #endregion
}
