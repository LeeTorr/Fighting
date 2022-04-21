using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartGUI : MonoBehaviour
{
    public Button AI;
    public Button Player;
    public GameObject TurnManager;
    public GameObject GuiManager;
    public GameObject PlayerGUI;

    void Start()
    {
        Button btn = AI.GetComponent<Button>();
        Button btn2 = Player.GetComponent<Button>();
        btn.onClick.AddListener(AiStart);
        btn2.onClick.AddListener(PlayerStart);
    }

    void PlayerStart()
    {
        TurnManager.GetComponent<TurnManager>().playerturn = true;
        TurnManager.GetComponent<TurnManager>().enemyturn = false;        
        GuiManager.SetActive(false);
        PlayerGUI.SetActive(true);

    }
    void AiStart()
    {
        TurnManager.GetComponent<TurnManager>().enemyturn = true;
        TurnManager.GetComponent<TurnManager>().playerturn = false;   
        GuiManager.SetActive(false);
    }
}
