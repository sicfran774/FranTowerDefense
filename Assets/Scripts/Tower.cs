using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Info/Basic Stats")]
    public string towerType;
    public float rangeRadius;
    public float fireRate;
    public float projectileSpeed;
    public int price;
    public bool specialTower;

    [Header("Type Of Ammo")]

    public GameObject ammo;

    [Header("Abilities")]

    public float abilityDuration;
    public float abilityCooldown;
    public bool abilityOnCooldown;
    public float secondsUntilCooldownDone;

    [Header("Upgrade Prices")]

    public int upgrade1_1Price;
    public int upgrade1_2Price;
    public int upgrade2_1Price;
    public int upgrade2_2Price;

    [Header("Tad Rock Buffs")]
    public bool buffed;
    public bool doubleMoney;
    public bool canShootAllTypes;

    [Header("Misc")]
    public int bulletsShot;
    private float originalFireRate;
    public LayerMask layer;

    private GameObject currentEnemy;
    private List<GameObject> enemies;

    private GameManager gameManager;
    private Upgrade upgrade;

    private GameObject rangeIndicator;
    private bool enemyWithinRange;
    public float timer;
    private float angle;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.towerCount++;

        rangeIndicator = transform.GetChild(1).gameObject;

        enemies = new List<GameObject>();
        upgrade = new Upgrade();

        transform.SetParent(GameObject.Find("Towers").transform);

        originalFireRate = fireRate;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!specialTower)
        {
            if (enemies.Count > 0)
            {
                enemyWithinRange = true;
                currentEnemy = enemies[0];
                //currentEnemy.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                enemyWithinRange = false;
            }

            if (enemyWithinRange && currentEnemy != null && timer > fireRate && GetComponent<PlaceTower>().placedTower) //Shoot enemy
            {
                ShootEnemy(currentEnemy);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, ~layer);
            if (hit.collider == GetComponent<CapsuleCollider2D>())
            {
                this.tag = "SelectedTower";
                ShowRange();
            }
            else if(hit.collider != null && hit.collider.tag == "Upgrade" || hit.collider != null && hit.collider.tag == "Target")
            {
                   // do nothing..for now
            }
            else
            {
                this.tag = "Tower";
                HideRange();
            }
        }

        if (GetComponent<CoopaHandler>() == null)
        {
            UpdateRange();
        }
        else if(GetComponent<PlaceTower>().placedTower)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public GameObject InstantiateAmmo(Transform transform)
    {
        GameObject projectile = Instantiate(ammo, transform);
        return projectile;
    }

    void ShootEnemy(GameObject enemy)
    {
        Vector2 direction = PointAtEnemy(ref enemy);
        //Debug.Log(direction);

        GameObject projectile = InstantiateAmmo(transform);
        if(canShootAllTypes) projectile.GetComponent<Projectile>().canShootAllTypes = true;

        projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);

        if (GetComponent<SpreadShot>() != null && GetComponent<SpreadShot>().activated)
        {
            GetComponent<SpreadShot>().ShootSpread(direction, projectileSpeed);
            bulletsShot += 2;
        }
        gameManager.PlayWhishNoise();

        bulletsShot++;
        timer = 0;
    }

    public Vector2 PointAtEnemy(ref GameObject enemy)
    {
        Vector3 targ = enemy.transform.position;
        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;

        FlipImage(angle, gameObject, transform.localScale.x);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return new Vector2(targ.x, targ.y);
    }

    void FlipImage(float angle, GameObject obj, float scale)
    {
        if(angle < -90 || angle > 90)
        {
            obj.transform.localScale = new Vector2(scale, -scale);
        }
        else
        {
            obj.transform.localScale = new Vector2(scale, scale);
        }
    }

    void UpdateRange()
    {
        if (GetComponent<PlaceTower>().placedTower)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (GetComponent<JrollHandler>() != null)
            {
                rangeIndicator.transform.localScale = new Vector3(rangeRadius * 10 + 0.8f, rangeRadius * 10 + 0.8f, 0);
                GetComponent<JrollHandler>().range = rangeRadius;
            }
            else
            {
                rangeIndicator.transform.localScale = new Vector3(rangeRadius * 2, rangeRadius * 2, 0);
                transform.GetComponentInChildren<CircleCollider2D>().radius = rangeRadius;
            }
        }
    }

    void ShowRange()
    {
        rangeIndicator.SetActive(true);
    }

    void HideRange()
    {
        rangeIndicator.SetActive(false);
    }

    public Upgrade GetUpgradeInstance()
    {
        return upgrade;
    }

    public void SetRange(float range)
    {
        rangeRadius = range;
    }

    public void SetFireRate(float fireRate)
    {
        this.fireRate = fireRate;
    }

    public void SetProjectile(GameObject ammo)
    {
        this.ammo = ammo;
    }

    public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public List<GameObject> GetEnemyList()
    {
        return enemies;
    }
    public float GetOriginalFireRate()
    {
        return originalFireRate;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            //collider.GetComponent<SpriteRenderer>().color = Color.red;
            enemies.Add(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Enemy" && enemies.Count > 0)
        {
            //collider.GetComponent<SpriteRenderer>().color = Color.white;
            enemies.Remove(collider.gameObject);
        }
    }

    public IEnumerator AbilityCooldown()
    {
        abilityOnCooldown = true;
        StartCoroutine(CooldownCountdown());
        yield return new WaitForSeconds(abilityCooldown);
        abilityOnCooldown = false;
    }

    public IEnumerator CooldownCountdown()
    {
        for (int i = 1; i <= abilityCooldown; i++)
        {
            yield return new WaitForSeconds(1f);
            secondsUntilCooldownDone = abilityCooldown - i;
        }
    }
    void OnDestroy()
    {
        gameManager.towerCount--;
    }
}
