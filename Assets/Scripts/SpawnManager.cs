using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  Format of spawning enemies
 *  
 *  ***Legend***
 *  a = normal abner
 *  i = ice abner
 *  f = fire abner
 *  
 *  endRound = End the round (must be last element for that round)
 *  
 *  **************
 *  Health of enemy must be an int directly after enemy letter
 *  i.e. Abner with 3 HP -->  "a3"
 *  **************
 *  Separate each enemy with an int, which will make the spawn manager wait that many seconds
 *  i.e. Spawn 3 Abners, 0.5 sec apart from each other --> "a", "0.5", "a", "0.5", "a"
 *  
 */

public class SpawnManager : MonoBehaviour
{
    public float defaultTimeInterval;
    public int defaultHealth;
    public int numEnemiesSpawned;

    [Space(20)]

    public GameManager gameManager;
    public GameObject enemyAbner;
    public GameObject enemyIceAbner;
    public GameObject enemyFireAbner;
    public GameObject enemyAbnerOgre;

    private string[] enemyOrder;
    private bool enemiesOnBoard;
    private int currentPosition;

    void Awake()
    {
        enemyOrder = gameManager.LoadLevelData();
        enemiesOnBoard = false;
    }

    void Update()
    {
        if (gameManager.startRound)
        {
            gameManager.startRound = false;
            Debug.Log("Begin Round " + gameManager.round);
            StartCoroutine(SpawnEnemy());
        }

        if (!GameObject.FindGameObjectWithTag("Enemy"))
        {
            enemiesOnBoard = false;
        }
        else
        {
            enemiesOnBoard = true;
        }
    }
    public void GoToRound(int round)
    {
        enemyOrder = gameManager.LoadLevelData();
        int count = 1;
        while (count <= round)
        {
            if (enemyOrder[currentPosition] == "endRound")
            {
                gameManager.round++;
                count++;
            }
            currentPosition++;
        }
    }

    IEnumerator SpawnEnemy()
    {
        numEnemiesSpawned = 0; //reset to zero, used later to calculate RewardEndOfRound() in GameManager
        while(true)
        {
            int health = defaultHealth;
            float time = defaultTimeInterval;
            GameObject gameObject = enemyAbner;

            CheckIfTimeGiven(ref time);

            //Start of conditions
            //Checks what letter/number it sees

            CheckHealthOfEnemy(ref health);
            if (enemyOrder[currentPosition].StartsWith("a"))
            {
                SpawnEnemy(health, enemyAbner);
            }
            if (enemyOrder[currentPosition].StartsWith("i"))
            {
                SpawnEnemy(health, enemyIceAbner);
            }
            if (enemyOrder[currentPosition].StartsWith("f"))
            {
                SpawnEnemy(health, enemyFireAbner);
            }
            if (enemyOrder[currentPosition].StartsWith("o"))
            {
                SpawnEnemy(health, enemyAbnerOgre);
            }
            

            //End the round
            if (enemyOrder[currentPosition] == "endRound")
            {
                Debug.Log("Finished spawning successfully.");
                currentPosition++;
                break;
            }

            //Increment string array for next element
            currentPosition++;

            yield return new WaitForSeconds(time); //Wait time, repeat until it reaches "End the round"
        }
        StartCoroutine(CheckIfEnemyOnBoard());
    }
    IEnumerator CheckIfEnemyOnBoard()
    {
        while (true)
        {
            if (!enemiesOnBoard)
            {
                break;
            }
            yield return null;
        }
        gameManager.RewardEndOfRound();
        gameManager.ClearAllProjectiles();
        gameManager.startRoundButton.interactable = true;
        gameManager.roundInProgress = false;
        if(enemyOrder.Length == currentPosition)
        {
            gameManager.Victory(GameObject.Find("GameUI").GetComponent<Player>().health);
            yield break;
        }
        gameManager.SavePlayerData();
        gameManager.SaveTowerData();
    }

    void CheckIfTimeGiven(ref float time)
    {
        bool isParsable = float.TryParse(enemyOrder[currentPosition], out time); //Try to parse text file into float
        if (isParsable) //If able to...
        {
            time = float.Parse(enemyOrder[currentPosition]); //Assign "time" variable to that float,  which will
        }                                                    //be used in WaitForSeconds before next enemy is spawned
        else
        {
            time = defaultTimeInterval; //default time interval between each enemy
        }
    }

    void CheckHealthOfEnemy(ref int health)
    {
        
        bool isParsable = int.TryParse(enemyOrder[currentPosition].Substring(1), out health); //check if there is a number after enemy (for health)
        if (isParsable)
        {
            health = int.Parse(enemyOrder[currentPosition].Substring(1)); //use that health
        }
        else
        {
            health = defaultHealth;
        }
        numEnemiesSpawned += health;
    }

    public void SandboxEnemySpawn(string type)
    {
        int health;
        bool isParsable = int.TryParse(GameObject.Find("EnemyHealth").GetComponent<Text>().text, out health);
        if (isParsable)
        {
            health = int.Parse(GameObject.Find("EnemyHealth").GetComponent<Text>().text); //use that health
        }
        else
        {
            health = 1;
        }

        switch (type)
        {
            case "Abner":
                SpawnEnemy(health, enemyAbner);
                break;
            case "IceAbner":
                SpawnEnemy(health, enemyIceAbner);
                break;
            case "FireAbner":
                SpawnEnemy(health, enemyFireAbner);
                break;
            case "BossAbner":
                SpawnEnemy(health, enemyAbnerOgre);
                break;
            default:
                break;
        }
    }

    public void SpawnEnemy(int health, GameObject type)
    {
        GameObject newObject = Instantiate(type);
        newObject.transform.SetParent(GameObject.Find("Enemies").transform);
        newObject.transform.position = this.transform.position;

        newObject.GetComponent<Enemy>().health = health;
        if(!newObject.GetComponent<Enemy>().boss) newObject.GetComponent<Enemy>().speed = health > 5 ? 7f : health + 1f;
    }

    public void SpawnEnemy(int health, GameObject type, Vector2 pos, string direction)
    {
        GameObject newObject = Instantiate(type);
        newObject.transform.SetParent(GameObject.Find("Enemies").transform);
        newObject.transform.position = new Vector3(pos.x, pos.y, 0);

        newObject.GetComponent<Enemy>().health = health;
        newObject.GetComponent<Enemy>().speed = health > 5 ? 7f : health + 1f;
        newObject.GetComponent<Enemy>().UpdateSpeed(direction);
    }

    public void StartBossDefeated(Transform transform, string direction)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        StartCoroutine(BossDefeated(pos, direction));
    }

    IEnumerator BossDefeated(Vector2 pos, string direction)
    {
        SpawnEnemy(8, enemyAbner, pos, direction);
        yield return new WaitForSeconds(0.05f);
        SpawnEnemy(8, enemyAbner, pos, direction);
        yield return new WaitForSeconds(0.05f);
        SpawnEnemy(8, enemyAbner, pos, direction);
        yield return new WaitForSeconds(0.05f);
        SpawnEnemy(8, enemyAbner, pos, direction);
        yield return new WaitForSeconds(0.05f);
    }
}
