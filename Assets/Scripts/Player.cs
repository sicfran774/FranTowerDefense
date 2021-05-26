using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private GameObject player;
    private Text healthText;
    private Text moneyText;
    private Text round;

    public int health;
    public int money;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthText = player.transform.GetChild(0).GetComponent<Text>();
        moneyText = player.transform.GetChild(1).GetComponent<Text>();
        round = player.transform.GetChild(2).GetComponent<Text>();
    }


    void Update()
    {
        healthText.text = health.ToString();
        moneyText.text = money.ToString();
        if(GameObject.Find("Game Manager").GetComponent<GameManager>().round != 0)
        {
            round.text = "Round " + GameObject.Find("Game Manager").GetComponent<GameManager>().round.ToString();
        }
    }
}
