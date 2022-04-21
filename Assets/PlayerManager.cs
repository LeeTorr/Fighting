using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public GameObject PlayerGUI;

    public GameObject TurnManager;
    public float attack1Ranges;
    public float attack1Range;
    public Button HealButton;
    public Button Attack2;
    public float speed = 3f;
    public Button Skip;
    public float attacktimer;
    public GameObject PlayerSpawn;
    public GameObject lookat;

    public Image EnemyHealthBar;
    public Text PlayerWins;

    public float healValue;
    public float attackValue = 0.5f;
    public Image PlayerHealthBar;

    public bool isattacking2;

    // Start is called before the first frame update
    void Start()
    {
        player.transform.LookAt(lookat.transform);
        Button btn = HealButton.GetComponent<Button>();
        btn.onClick.AddListener(Heal);
        Button btn2 = Attack2.GetComponent<Button>();
        btn2.onClick.AddListener(AttackTwoStart);
        Button btn3 = Skip.GetComponent<Button>();
        btn3.onClick.AddListener(SkipTurn);


        player = GameObject.FindGameObjectWithTag("Player");
       // enemy = GameObject.FindGameObjectWithTag("Enemy");
        //При старте нет атаки
        isattacking2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyHealthBar.fillAmount <= 0.01f){ 

           Destroy(enemy);
            PlayerWins.gameObject.SetActive(true);
        }

        attack1Ranges = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (Vector3.Distance(player.transform.position, enemy.transform.position) > attack1Range && isattacking2 == true)
        {
            player.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }
        //При условии что враг близко, производится атака 
        if (attack1Ranges <= 8 && isattacking2 == true)
        {
            player.transform.Rotate(Vector3.up * Time.deltaTime * 445, Space.World);
            //При атаке, меняем значение переменной  
            Attack();

        }

    }
    void Heal()
    {
        PlayerHealthBar.fillAmount += healValue;
        Enemyturn();

    }
    void AttackTwoStart()
    {
        isattacking2 = true;

    }
    void Attack()
    {
        //Таймер атаки 
        attacktimer += Time.deltaTime;
        if (attacktimer >= 0.7)
        {
            EnemyHealthBar.fillAmount -= attackValue;
            isattacking2 = false;
            Comeback();

        }
    }

    void Comeback()
    {
        //Возвращение врага на свой спавн 
        player.transform.LookAt(lookat.transform);
        player.transform.position = PlayerSpawn.transform.position;
        //isattacking2 = false;
        attacktimer = 0.0f;
        Enemyturn();


    }
    void SkipTurn()
    {


    }

    void Enemyturn()
    {
        //Переключаем ход на врага
        TurnManager.GetComponent<TurnManager>().playerturn = false;
        TurnManager.GetComponent<TurnManager>().enemyturn = true;
        PlayerGUI.SetActive(false);

    }
}
