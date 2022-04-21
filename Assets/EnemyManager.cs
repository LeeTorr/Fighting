using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;
    public GameObject lookat;

    public GameObject enemy;
    public GameObject TurnManager;
    public float speed = 3f;
    public float attack1Range;
    public float attack1Ranges;
    public GameObject EnemySpawn;
    public GameObject PlayerSpawn;
    public GameObject PlayerGUI;

    //Востановление здоровья
    public GameObject HealCircle;
    public float healtimer;

    public float hp;
    public float endDamageRange;
    public float startDamageRange = 0.1f;
    public float healValue = 0.8f;
    public float attacktimer;
    //При старте нет атаки
    public bool aiturn = true;
    public bool heal = false;
    public bool isAttacking = false;

    public Image PlayerHealthBar;
    public Image EnemyHealthBar;
    public Text EnemyWins;

    private float[] turnBasedDamages = new float[5];
    private int turnCount = 0;
    private int defaultDepth = 5;
    public PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        enemy.transform.LookAt(lookat.transform);
        GenerateDamages();
    }
    // Update is called once per frame
    void Update()
    {
        hp = EnemyHealthBar.fillAmount;
        attack1Ranges = Vector3.Distance(player.transform.position, enemy.transform.position);
      
        //При условии что враг близко, производится атака 
        if (aiturn)
        {        
            //при выборе меняем переменные
            if (makeHealMove())
            {            
                heal = true;
            }
            else
            {                      
                isAttacking = true;
            }
        }
        if (heal)
        {
            isAttacking = false;
            Heal();
        }
        if (isAttacking)
        {
            heal = false;
            if (Vector3.Distance(player.transform.position, enemy.transform.position) > attack1Range && aiturn)
            {
                enemy.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            }
            if (attack1Ranges <= 8 && aiturn) { 
                Attack();
            }
        }

    }
    void Attack()
    {
        //Таймер атаки 
        enemy.transform.Rotate(Vector3.up * Time.deltaTime * 445, Space.World);
        attacktimer += Time.deltaTime;
        if (attacktimer >= 0.7)
        {
            if (turnCount >= turnBasedDamages.Length)
            {
                turnCount = 0;
            }
            PlayerHealthBar.fillAmount -= turnBasedDamages[turnCount];
            aiturn = false;
            Comeback();
        }
        
    }
    void Comeback()
    {
        //Возвращение врага на свою позицию
        attacktimer = 0.0f;
        healtimer = 0.0f;
        heal = false;
        isAttacking = false;

        enemy.transform.LookAt(lookat.transform);
        enemy.transform.position = EnemySpawn.transform.position;
        Playerturn();
    }
    void Playerturn()
    {
        //Переключаем ход на игрока после атаки
        turnCount++;
        aiturn = true;
        PlayerGUI.SetActive(true);
        TurnManager.GetComponent<TurnManager>().playerturn = true;
        TurnManager.GetComponent<TurnManager>().enemyturn = false;
    }
    void Heal()
    {
        enemy.transform.Rotate(Vector3.up * Time.deltaTime * 445, Space.World);
        healtimer += Time.deltaTime;
         
        if (healtimer >= 0.7)
        {
            EnemyHealthBar.fillAmount += healValue;
            //Playerturn();
            Comeback();
        }
    }

    private void GenerateDamages()
    {
        for (int index = 0; index < turnBasedDamages.Length; index++)
        {
            turnBasedDamages[index] = UnityEngine.Random.Range(startDamageRange, endDamageRange);
        }
    }

    private bool makeHealMove()
    {
        float currentHp = EnemyHealthBar.fillAmount;
        int currentEnemyTurn = turnCount;
        float playerHp = playerManager.PlayerHealthBar.fillAmount;

        int healTurnValue = minimax(currentHp + healValue, playerHp, currentEnemyTurn + 1, defaultDepth, false);
        if (currentEnemyTurn + 1 >= turnBasedDamages.Length)
        {
            currentEnemyTurn = -1;
        }
        int damageTurnValue = minimax(currentHp, playerHp - turnBasedDamages[currentEnemyTurn + 1], currentEnemyTurn + 1, defaultDepth, false);
        if (damageTurnValue >= healTurnValue)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private int minimax(float currentHp, float playerHp, int enemyTurNumber, int depth, bool isMax)
    {
        int score = evaluate(currentHp, playerHp);

        switch (score)
        {
            case 10: return score - depth;
            case -10: return score + depth;
        }

        if (depth == 0)
        {
            return 0;
        }

        if (isMax)
        {
            int bestValue = int.MinValue;
            float newHp = currentHp + healValue;
            int newTurn = enemyTurNumber;
            bestValue = Math.Max(bestValue, minimax(newHp, playerHp, newTurn + 1, depth--, !isMax));
            if (enemyTurNumber + 1 >= turnBasedDamages.Length)
            {
                newTurn = -1;
            }
            bestValue = Math.Max(bestValue, minimax(currentHp, playerHp - turnBasedDamages[newTurn + 1], newTurn + 1, depth--, !isMax));

            return bestValue;
        }
        else
        {
            int worstValue = int.MaxValue;

            float newHp = currentHp + playerManager.healValue;
            int newTurn = enemyTurNumber;
            worstValue = Math.Min(worstValue, minimax(currentHp, newHp, enemyTurNumber, depth--, !isMax));
            worstValue = Math.Min(worstValue, minimax(currentHp - playerManager.attackValue, playerHp, enemyTurNumber, depth--, !isMax));

            return worstValue;
        }
    }

    private int evaluate(float currentHp, float playerHp)
    {
        //if (currentHp > playerHp)
        if (currentHp > playerHp && currentHp > playerManager.attackValue)
        {
            return 10;
        }
        else
        {
            return -10;
        }

        return 0;
    }
}
