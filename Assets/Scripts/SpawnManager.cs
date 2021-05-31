﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Format of spawning enemies
 *  
 *  ***Legend***
 *  a = abner
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

    private string[] enemyOrder;
    private int currentPosition;

    void Awake()
    {
        enemyOrder = gameManager.LoadLevelData();
    }

    void Update()
    {
        if (gameManager.startRound)
        {
            gameManager.startRound = false;
            Debug.Log("Begin Round " + gameManager.round);
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        numEnemiesSpawned = 0; //reset to zero, used later to calculate RewardEndOfRound() in GameManager
        while(true)
        {
            int health = defaultHealth;
            float time = defaultTimeInterval;

            CheckIfTimeGiven(ref time);

            //Start of conditions
            //Checks what letter/number it sees

            //Abner
            if(enemyOrder[currentPosition].StartsWith("a"))
            {
                CheckHealthOfEnemy(ref health);
                SpawnEnemyAbner(health);
            }

            //End the round
            if(enemyOrder[currentPosition] == "endRound")
            {
                Debug.Log("Finished spawning successfully.");
                currentPosition++;
                break;
            }

            //Increment string array for next element
            currentPosition++;

            yield return new WaitForSeconds(time); //Wait time, repeat until it reaches "End the round"
        }
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

    public void SpawnEnemyAbner(int health)
    {
        GameObject newObject = Instantiate(enemyAbner);
        newObject.transform.SetParent(GameObject.Find("Enemies").transform);
        newObject.transform.position = this.transform.position;

        newObject.GetComponent<Enemy>().health = health;
        newObject.GetComponent<Enemy>().speed = health > 5 ? 5.5f : health + 0.5f;
    }
}
