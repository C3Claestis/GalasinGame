using UnityEngine;
using UnityEngine.UI;

public class CovenantManager : MonoBehaviour
{
    [SerializeField] int enemyRound;
    [SerializeField] Text covenantText;
    [SerializeField] Text loseText;
    
    private DrawPathMovement[] players;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (players == null || players.Length == 0)
        {
            players = FindObjectsByType<DrawPathMovement>(FindObjectsSortMode.None);
        }

        covenantText.text = players.Length.ToString();
        loseText.text = enemyRound.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
