using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool playerturn;
    public bool enemyturn;
  //  public GameObject TurnManager;
    public GameObject enemy;
    public GameObject player;
    public Image PlayerHealthBar;
    public Image EnemyHealthBar;
    public Text EnemyWins;
    public GameObject PlayerGUI;
    public GameObject restartGUI;


    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        playerturn = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealthBar.fillAmount <= 0.01f)
        {
            Destroy(player);
            EnemyWins.gameObject.SetActive(true);
            PlayerGUI.SetActive(false);
            restartGUI.SetActive(true);

        }
        if (EnemyHealthBar.fillAmount <= 0.00f)
        {
      
            restartGUI.SetActive(true);

        }
        if (playerturn)
            enemy.GetComponent<EnemyManager>().enabled = false;
      
        if (enemyturn)
            enemy.GetComponent<EnemyManager>().enabled = true;
        //пропустить ход
    
    }
}
