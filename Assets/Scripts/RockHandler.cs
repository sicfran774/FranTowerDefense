using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHandler : MonoBehaviour
{
    public List<GameObject> towers;
    private int numTowers;

    void Start()
    {
        towers = new List<GameObject>();
    }

    void Update()
    {
        if (this.tag == "SelectedTower")
        {
            HighlightTowersInList();
        }
        else
        {
            RevertTowerColors();
        }

        UpdateTowerList();
        ApplyBuffs();
    }

    void ApplyBuffs()
    {
        foreach (GameObject tower in towers)
        {
            if(GetComponent<Tower>().GetUpgradeInstance().GetUpgradeLevel(1) == 2 && !tower.GetComponent<Tower>().doubleMoney)
            {
                tower.GetComponent<Tower>().doubleMoney = true;
            }
            else if(GetComponent<Tower>().GetUpgradeInstance().GetUpgradeLevel(1) == 3 && !tower.GetComponent<Tower>().canShootAllTypes)
            {
                tower.GetComponent<Tower>().canShootAllTypes = true;
            }
        }
    }

    int UpdateTowerList()
    {
        towers.Clear();

        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            if (transform.GetChild(0).GetComponent<CircleCollider2D>().bounds.Intersects(tower.GetComponent<CapsuleCollider2D>().bounds))
            {
                if (tower == this.gameObject)
                {
                    continue;
                }
                else
                {
                    towers.Add(tower);
                }
            }
        }

        GameObject selectedTower = GameObject.FindGameObjectWithTag("SelectedTower");
        if (selectedTower != null && selectedTower != this.gameObject && transform.GetChild(0).GetComponent<CircleCollider2D>().bounds.Intersects(selectedTower.GetComponent<CapsuleCollider2D>().bounds))
        {
            towers.Add(selectedTower);
        }

        return towers.Count;
    }
    void HighlightTowersInList()
    {
        foreach(GameObject tower in towers)
        {
            tower.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    void RevertTowerColors()
    {
        foreach (GameObject tower in towers)
        {
            tower.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
