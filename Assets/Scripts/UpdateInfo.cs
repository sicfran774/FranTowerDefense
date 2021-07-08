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
                    break;
                case "Coopa":
                    currentTower = Coopa;
                    break;
                case "Juuls":
                    currentTower = Juuls;
                    break;
                case "Tad":
                    currentTower = Tad;
                    break;
                case "Jroll":
                    currentTower = Jroll;
                    break;
                case "SuperFran":
                    currentTower = SuperFran;
                    break;
            }
            UpdateTowerData(currentTower);
        }
    }
}
