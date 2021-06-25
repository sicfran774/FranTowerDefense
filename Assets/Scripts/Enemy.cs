using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public int reward;
    public string direction;
    public bool burning;
    public bool slowed;
    public bool frozen;

    private bool hit;

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

        spriteFire.SetActive(false);
        spriteIce.SetActive(false);
    }

    void Update()
    {
        if (!frozen)
        {
            UpdateSpeed();
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

        if (health <= 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
        
        ChangeColor(health);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!hit && collider.tag == "Projectile")
        {
            if (collider.GetComponent<Projectile>().damage > 0)
            {
                hit = true;
            }
            if (collider.GetComponent<Projectile>().burn && !burning)
            {
                burning = true;
                StartCoroutine(BurnTick(collider.GetComponent<Projectile>().burnTick, collider.GetComponent<Projectile>().tickDamage, collider.GetComponent<Projectile>().tickInterval));
            }
            if (collider.GetComponent<Projectile>().slow && !slowed)
            {
                slowed = true;
                StartCoroutine(SlowTick(collider.GetComponent<Projectile>().slowTick, collider.GetComponent<Projectile>().tickDamage, collider.GetComponent<Projectile>().tickInterval));
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
            GameObject.Find("GameUI").GetComponent<Player>().health -= health;
        }
    }

    void UpdateSpeed()
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
        }
    }
    void UpdateSpeedAfterHit()
    {
        speed = health > 5 ? 5.5f : health + 0.5f;
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

        burning = false;
        spriteFire.SetActive(false);
    }

    IEnumerator SlowTick(int time, int damage, float interval)
    {
        spriteIce.SetActive(true);
        speed /= 2;

        for (int tick = 0; tick < time; tick++)
        {
            yield return new WaitForSeconds(interval);
            if (damage > 0)
            {
                health -= damage;
                hit = true;
                gameManager.PlayPopNoise();
            }
        }

        UpdateSpeedAfterHit();
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
