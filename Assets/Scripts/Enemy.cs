using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public int reward;

    [Header("Special Enemy")]
    public bool ice;
    public bool fire;
    public bool boss;

    public string direction;

    [Header("States")]
    public bool burning;
    public bool flamed;
    public bool slowed;
    public bool frozen;

    private bool hit;
    private float tempSpeed;

    private GameManager gameManager;

    private Rigidbody2D rb;

    private SpriteRenderer sprite;
    private GameObject spriteFire;
    private GameObject spriteIce;
    private Color color;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>();
        spriteFire = transform.GetChild(0).gameObject;
        spriteIce = transform.GetChild(1).gameObject;

        color = GetComponent<SpriteRenderer>().color;

        hit = false;
        tempSpeed = speed;
        spriteFire.SetActive(false);
        spriteIce.SetActive(false);
    }

    void Update()
    {
        if (!boss)
        {
            ChangeColor(health);
            if (!frozen)
            {
                UpdateSpeed(direction);
            }

            if (hit)
            {
                hit = false;
                RewardMoney();

                if (!slowed)
                {
                    UpdateSpeedAfterHit();
                }
            }
        }
        else
        {
            ChangeDirection();
        }

        if (health <= 0)
        {
            StopAllCoroutines();
            if (boss)
            {
                GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnManager>().StartBossDefeated(transform, direction);
            }
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!hit && collider.tag == "Projectile")
        {
            Projectile projectile = collider.GetComponent<Projectile>();
            if (ice)
            {
                IceEnemyTrigger(projectile);
            }
            else if (fire)
            {
                FireEnemyTrigger(projectile);
            }
            else
            {
                NormalEnemyTrigger(projectile);
            }
        }

        if (rb != null)
        {
            Direction(collider);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void NormalEnemyTrigger(Projectile projectile)
    {
        if (projectile.damage > 0)
        {
            hit = true;
        }
        if (projectile.burn && !burning)
        {
            burning = true;
            StartCoroutine(BurnTick(projectile.burnTick, projectile.tickDamage, projectile.tickInterval));
        }
        if (projectile.slow && !slowed)
        {
            slowed = true;
            StartCoroutine(SlowTick(projectile.slowTick, projectile.tickDamage, projectile.tickInterval));
        }
    }

    void IceEnemyTrigger(Projectile projectile)
    {
        if (projectile.burn || projectile.canShootAllTypes)
        {
            NormalEnemyTrigger(projectile);
        }
    }
    void FireEnemyTrigger(Projectile projectile)
    {
        if (projectile.slow || projectile.canShootAllTypes)
        {
            NormalEnemyTrigger(projectile);
        }
    }

    void RewardMoney()
    {
        GameObject.Find("GameUI").GetComponent<Player>().money += reward;
    }

    void Direction(Collider2D collider)
    {
        if (collider.gameObject.name == "PathUp")
        {
            MoveUp();
        }
        if (collider.gameObject.name == "PathDown")
        {
            MoveDown();
        }
        if (collider.gameObject.name == "PathLeft")
        {
            MoveLeft();
        }
        if (collider.gameObject.name == "PathRight")
        {
            MoveRight();
        }
        if (collider.gameObject.name == "DestroyBlock")
        {
            Destroy(gameObject);
            if (gameManager.sceneName == "Sandbox") return;

            if (!boss)
            {
                GameObject.Find("GameUI").GetComponent<Player>().health -= health;
            }
            else
            {
                GameObject.Find("GameUI").GetComponent<Player>().health -= 100;
            }
        }
    }

    public void UpdateSpeed(string direction)
    {
        switch (direction)
        {
            case "up":
                MoveUp();
                break;
            case "down":
                MoveDown();
                break;
            case "left":
                MoveLeft();
                break;
            case "right":
                MoveRight();
                break;
        }
    }

    void MoveUp()
    {
        rb.velocity = new Vector3(0, speed, 0);
        direction = "up";
    }

    void MoveDown()
    {
        rb.velocity = new Vector3(0, -speed, 0);
        direction = "down";
    }

    void MoveLeft()
    {
        rb.velocity = new Vector3(-speed, 0, 0);
        direction = "left";
    }
    void MoveRight()
    {
        rb.velocity = new Vector3(speed, 0, 0);
        direction = "right";
    }

    void ChangeDirection()
    {
        switch (direction)
        {
            case "up":
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case "down":
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
            case "left":
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case "right":
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
        }
    }

    void ChangeColor(int health)
    {
        switch (health)
        {
            case 1:
                sprite.color = color;
                break;
            case 2:
                sprite.color = Color.blue;
                break;
            case 3:
                sprite.color = Color.green;
                break;
            case 4:
                sprite.color = Color.magenta;
                break;
            case 5:
                sprite.color = Color.cyan;
                break;
            case 6:
                sprite.color = Color.yellow;
                break;
            case 7:
                sprite.color = Color.gray;
                break;
            default:
                sprite.color = Color.black;
                break;
        }
    }
    void UpdateSpeedAfterHit()
    {
        speed = health > 5 ? 7f : health + 1f;
    }

    public void StartBurnTick(int time, int damage, float interval)
    {
        StartCoroutine(BurnTick(time, damage, interval));
    }

    IEnumerator BurnTick(int time, int damage, float interval)
    {
        spriteFire.SetActive(true);

        for(int tick = 0; tick < time; tick++)
        {
            yield return new WaitForSeconds(interval);
            health -= damage;
            hit = true;
            gameManager.PlayPopNoise();
        }

        flamed = false;
        burning = false;
        spriteFire.SetActive(false);
    }

    IEnumerator SlowTick(int time, int damage, float interval)
    {
        spriteIce.SetActive(true);
        speed /= 2;

        for (int tick = 0; tick < time; tick++)
        {
            UpdateSpeed(direction);
            yield return new WaitForSeconds(interval);
            if (damage > 0)
            {
                health -= damage;
                hit = true;
                gameManager.PlayPopNoise();
            }
        }

        if (!boss)
        {
            UpdateSpeedAfterHit();
        }
        else
        {
            speed = tempSpeed;
            UpdateSpeed(direction);
        }
        slowed = false;

        if (!frozen)
        {
            spriteIce.SetActive(false);
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
