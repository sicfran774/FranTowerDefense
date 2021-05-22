using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    GameObject parent;
    Text healthText;
    Text moneyText;
    Text round;

    public int health;
    public int money;

    void Awake()
    {
        parent = transform.GetChild(0).gameObject;
        healthText = parent.GetComponent<Text>();
        parent = transform.GetChild(2).gameObject;
        moneyText = parent.GetComponent<Text>();
        parent = transform.GetChild(4).gameObject;
        round = parent.GetComponent<Text>();
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
