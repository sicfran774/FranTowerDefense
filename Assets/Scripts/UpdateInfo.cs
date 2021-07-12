using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateInfo : MonoBehaviour
{
    public GameObject Pog;
    public GameObject Coopa;
    public GameObject Juuls;
    public GameObject Tad;
    public GameObject Jroll;
    public GameObject SuperFran;

    private GameObject currentTower;
    private GameObject towerInfoUI;

    private bool mouseOver;

    void Start()
    {
        towerInfoUI = transform.GetChild(1).gameObject;
        HideTowerInfo();
    }

    void Update()
    {
        currentTower = GameObject.FindGameObjectWithTag("SelectedTower");

        if (currentTower != null && !currentTower.GetComponent<PlaceTower>().placedTower)
        {
            UpdateTowerData(currentTower);
            ShowTowerInfo();
        }
        else if (!mouseOver)
        {
            HideTowerInfo();
        }
    }

    void ShowTowerInfo()
    {
        towerInfoUI.SetActive(true);
    }

    public void HideTowerInfo()
    {
        mouseOver = false;
        towerInfoUI.SetActive(false);
    }

    void UpdateTowerData(GameObject currentTower)
    {
        towerInfoUI.transform.GetChild(0).GetComponent<Text>().text = currentTower.GetComponent<Tower>().price.ToString();
        towerInfoUI.transform.GetChild(1).GetComponent<Text>().text = currentTower.GetComponent<Tower>().towerType;
        CheckIfAfford(GetComponent<Player>().money, currentTower.GetComponent<Tower>().price);
    }
    void CheckIfAfford(int money, int price)
    {
        if (money < price)
        {
            towerInfoUI.transform.GetChild(0).GetComponent<Text>().color = Color.red;
        }
        else
        {
            towerInfoUI.transform.GetChild(0).GetComponent<Text>().color = Color.green;
        }
    }

    public void MouseOverButton(string tower)
    {
        if (GameObject.FindGameObjectWithTag("SelectedTower") == null)
        {
            ShowTowerInfo();
            mouseOver = true;
            switch (tower)
            {
                case "Pog":
                    currentTower = Pog;
                    towerInfoUI.transform.GetChild(3).GetComponent<Text>().text = "The basic tower. Shoots Frans at the enemy.";
                    break;
                case "Coopa":
                    currentTower = Coopa;
                    towerInfoUI.transform.GetChild(3).GetComponent<Text>().text = "Spews flames at certain intervals, then cools down. Sets Abners on fire, which will take damage over time. Use the target to aim the Coopa. Effective against Ice Abners.";
                    break;
                case "Juuls":
                    currentTower = Juuls;
                    towerInfoUI.transform.GetChild(3).GetComponent<Text>().text = "Shoots ice cubes because he's ice cold, which slows Abners down. Effective against Fire Abners.";
                    break;
                case "Tad":
                    currentTower = Tad;
                    towerInfoUI.transform.GetChild(3).GetComponent<Text>().text = "Buffs the firerate of towers within the radius. Upgrades provide further buffs and more money.";
                    break;
                case "Jroll":
                    currentTower = Jroll;
                    towerInfoUI.transform.GetChild(3).GetComponent<Text>().text = "Periodically puts Jrolls onto the path. Can be used as a last line of defense.";
                    break;
                case "SuperFran":
                    currentTower = SuperFran;
                    towerInfoUI.transform.GetChild(3).GetComponent<Text>().text = "The Lerg's brother. Shoots Frans at an extremely fast rate.";
                    break;
            }
            UpdateTowerData(currentTower);
        }
    }
}
