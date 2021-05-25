using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateInfo : MonoBehaviour
{
    private GameObject currentTower;
    private GameObject towerInfoUI;

    void Start()
    {
        towerInfoUI = GameObject.Find("GameUI").transform.GetChild(1).gameObject;
        HideTowerInfo();
    }

    void Update()
    {
        currentTower = GameObject.FindGameObjectWithTag("SelectedTower");

        if (currentTower != null && !currentTower.GetComponent<PlaceTower>().placedTower)
        {
            UpdateTowerData();
            ShowTowerInfo();
        }
        else
        {
            HideTowerInfo();
        }
    }

    void ShowTowerInfo()
    {
        towerInfoUI.SetActive(true);
    }

    void HideTowerInfo()
    {
        towerInfoUI.SetActive(false);
    }

    void UpdateTowerData()
    {
        towerInfoUI.transform.GetChild(0).GetComponent<Text>().text = currentTower.GetComponent<Tower>().price.ToString();
        towerInfoUI.transform.GetChild(1).GetComponent<Text>().text = currentTower.GetComponent<Tower>().towerType;
    }
}
