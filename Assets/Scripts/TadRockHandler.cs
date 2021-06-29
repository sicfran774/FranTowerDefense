using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadRockHandler : MonoBehaviour
{
    public bool passiveIncome;

    private List<GameObject> towers;
    private GameManager gameManager;

    private float timer;
    public float range;

    void Start()
    {
        towers = new List<GameObject>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (this.tag == "SelectedTower" && GetComponent<PlaceTower>().placedTower)
        {
            HighlightTowersInList();
        }
        else
        {
            RevertTowerColors();
        }

        UpdateTowerList();
        ApplyBuffs();

        if (gameManager.roundInProgress && timer > GetComponent<Tower>().fireRate && passiveIncome)
        {
            GenerateRock();
            timer = 0;
        }
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

    void RemoveAllBuffs()
    {
        foreach (GameObject tower in towers)
        {
            tower.GetComponent<SpriteRenderer>().color = Color.white;
            tower.GetComponent<Tower>().doubleMoney = false;
            tower.GetComponent<Tower>().canShootAllTypes = false;
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
    void GenerateRock()
    {
        GameObject rock = GetComponent<Tower>().InstantiateAmmo(transform);
        rock.transform.position = transform.position;
        rock.GetComponent<Rigidbody2D>().AddForce(GenerateRandomLine() * GetComponent<Tower>().projectileSpeed);
    }
    Vector2 GenerateRandomLine()
    {
        float x = Random.Range(0, range);
        float y = MaintainHypotnuse(ref x);
        return new Vector2(x, y);
    }

    float MaintainHypotnuse(ref float x)
    {
        float y = Mathf.Sqrt(Mathf.Pow(range, 2) - Mathf.Pow(x, 2));

        int rand = Random.Range(0, 4);
        if (rand == 0)
        {
            x = -x;
        }
        else if (rand == 1)
        {
            y = -y;
        }
        else if (rand == 2)
        {
            y = -y;
            x = -x;
        }
        return y;
    }

    void OnDestroy()
    {
        RemoveAllBuffs();
    }
}
