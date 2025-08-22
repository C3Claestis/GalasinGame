using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject panelMenu;
    [SerializeField] GameObject panelMode;
    [SerializeField] GameObject panelAttacker;
    [SerializeField] GameObject panelOffender;
    [SerializeField] GameObject panelAttackBelakangRumah;
    [SerializeField] GameObject panelAttackHutanBelantara;

    [SerializeField] GameObject silangIconMusic;
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SFX;

    private void Start()
    {        
        // Ambil data dari PlayerPrefs, default = 1 (nyala)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        BGM.volume = savedVolume;
        SFX.volume = savedVolume;

        // Update icon sesuai kondisi
        silangIconMusic.SetActive(BGM.volume == 0);        
    }

    public void OnPlay()
    {
        panelMenu.SetActive(false);
        panelMode.SetActive(true);
    }

    public void OnAttacker()
    {
        panelMode.SetActive(false);
        panelAttacker.SetActive(true);
    }

    public void OnOffender()
    {
        panelMode.SetActive(false);
        panelOffender.SetActive(true);
    }

    public void GoScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneIndex, 2f));
    }

    private IEnumerator LoadSceneAfterDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

    public void ToggleMusic()
    {
        if (BGM.volume > 0)
        {
            BGM.volume = 0;
            SFX.volume = 0;
            silangIconMusic.SetActive(true);
        }
        else
        {
            BGM.volume = 0.4f;
            SFX.volume = 1;
            silangIconMusic.SetActive(false);
        }

        // Simpan ke PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", BGM.volume);
        PlayerPrefs.Save();
    }
}
