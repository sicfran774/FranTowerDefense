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

    private bool hit;

    private GameObject gameManager;

    private Rigidbody2D rb;

    private SpriteRenderer sprite;
    private GameObject spriteFire;

    private CircleCollider2D circle;
    private Color color;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");

        rb = GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>();
        spriteFire = transform.GetChild(0).gameObject;

        color = GetComponent<SpriteRenderer>().color;
        circle = GetComponent<CircleCollider2D>();

        hit = false;

        spriteFire.SetActive(false);
    }

    void Update()
    {
        if (hit)
        {
            hit = false;
            RewardMoney();
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
        ChangeColor(health);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!hit && collider.tag == "Projectile")
        {
            /*int burnTick = GetComponent<FranProjectile>().burnTick;
            int damage = GetComponent<FranProjectile>().damage;*/

            hit = true;
            if (collider.GetComponent<FranProjectile>().burn && !burning)
            {
                burning = true;
                StartCoroutine(BurnTick(collider.GetComponent<FranProjectile>().burnTick, collider.GetComponent<FranProjectile>().damage));
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
        if (collider.gameObject.name == "PathUp" || direction == "up")
        {
            MoveUp();
        }
        if (collider.gameObject.name == "PathDown" || direction == "down")
        {
            MoveDown();
        }
        if (collider.gameObject.name == "PathLeft" || direction == "left")
        {
            MoveLeft();
        }
        if (collider.gameObject.name == "PathRight" || direction == "right")
        {
            MoveRight();
        }
        if (collider.gameObject.name == "DestroyBlock")
        {
            Destroy(gameObject);
            GameObject.Find("GameUI").GetComponent<Player>().health -= health;
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

    IEnumerator BurnTick(int time, int damage)
    {
        spriteFire.SetActive(true);

        for(int tick = 0; tick < time; tick++)
        {
            yield return new WaitForSeconds(1.5f);
            health -= damage;
            hit = true;
            gameManager.GetComponent<GameManager>().PlayPopNoise(gameObject);
        }

        burning = false;
        spriteFire.SetActive(false);
    }
}
