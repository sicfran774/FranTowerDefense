using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadRockHandler : MonoBehaviour
{
    public bool passiveIncome;
    public LayerMask layer;
    public static float fireRateMultiplier = 0.8f;

    private List<GameObject> towers;
    private GameManager gameManager;
    private UpgradeManager upgradeManager;

    private float timer;
    public float range;

    void Start()
    {
        towers = new List<GameObject>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        upgradeManager = GameObject.Find("Upgrade Manager").GetComponent<UpgradeManager>();
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

        if (GetComponent<PlaceTower>().placedTower)
        {
            UpdateTowerList();
            ApplyBuffs();
        }

        if (gameManager.roundInProgress && timer > GetComponent<Tower>().fireRate && passiveIncome)
        {
            GenerateRock();
            timer = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layer);

            if (hit.collider != null)
            {
                GameObject.Find("GameUI").GetComponent<Player>().money += hit.collider.gameObject.GetComponent<RockHandler>().reward;
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void ApplyBuffs()
    {
        foreach (GameObject tower in towers)
        {
            if(tower == null)
            {
                continue;
            }
            Tower t = tower.GetComponent<Tower>();
            if (!t.buffed)
            {
                t.buffed = true;
                t.fireRate *= fireRateMultiplier;

                if(t.GetComponent<CoopaHandler>() != null)
                {
                    t.GetComponent<CoopaHandler>().cooldownDuration *= fireRateMultiplier;
                }
            }
            if(GetComponent<Tower>().GetUpgradeInstance().GetUpgradeLevel(1) == 2)
            {
                t.doubleMoney = true;
            }
            else if(GetComponent<Tower>().GetUpgradeInstance().GetUpgradeLevel(1) > 2)
            {
                t.doubleMoney = true;
                t.canShootAllTypes = true;
            }
        }
    }

    void RemoveAllBuffs()
    {
        foreach (GameObject tower in towers)
        {
            if (tower != null)
            {
                tower.GetComponent<SpriteRenderer>().color = Color.white;

                Tower t = tower.GetComponent<Tower>();

                t.buffed = false;
                t.GetComponent<Tower>().fireRate = t.GetOriginalFireRate();
                if(t.GetComponent<CoopaHandler>() != null)
                {
                    t.GetComponent<CoopaHandler>().cooldownDuration = t.GetComponent<CoopaHandler>().GetOriginalCooldownDuration();
                }

                upgradeManager.currentTower = tower;
                upgradeManager.UnlockUpgradeForSpecificTower(t.towerType, 1, t.GetUpgradeInstance().GetUpgradeLevel(1) - 1);
                upgradeManager.UnlockUpgradeForSpecificTower(t.towerType, 2, t.GetUpgradeInstance().GetUpgradeLevel(2) - 1);

                tower.GetComponent<Tower>().doubleMoney = false;
                tower.GetComponent<Tower>().canShootAllTypes = false;
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
            if(tower != null) tower.GetComponent<SpriteRenderer>().color = Color.white;
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
