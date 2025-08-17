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
}
