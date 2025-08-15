using UnityEngine;

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
    
    public void GoPractice()
    {

    }
    
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
}
