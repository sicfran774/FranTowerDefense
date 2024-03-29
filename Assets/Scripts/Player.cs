﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private GameObject player;
    private Text healthText;
    private Text moneyText;
    private Text roundText;

    public string level;
    public int health;
    public int money;
    public int round;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        healthText = player.transform.GetChild(0).GetComponent<Text>();
        moneyText = player.transform.GetChild(1).GetComponent<Text>();
        roundText = player.transform.GetChild(2).GetComponent<Text>();
    }


    void Update()
    {
        level = SceneManager.GetActiveScene().name;
        round = GameObject.Find("Game Manager").GetComponent<GameManager>().round;

        healthText.text = health.ToString();
        moneyText.text = money.ToString();
        if(GameObject.Find("Game Manager").GetComponent<GameManager>().round != 0)
        {
            roundText.text = "Round " + GameObject.Find("Game Manager").GetComponent<GameManager>().round.ToString();
        }
    }
}
