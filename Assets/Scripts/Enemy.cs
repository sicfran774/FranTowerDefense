using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public int reward;
    public string direction;

    private bool hit;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private CircleCollider2D circle;
    private Color color;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        color = GetComponent<SpriteRenderer>().color;
        circle = GetComponent<CircleCollider2D>();

        hit = false;
    }

    void Update()
    {
        if (hit)
        {
            RewardMoney();
            hit = false;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
        ChangeColor(health);
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(!hit && collider.tag == "Projectile")
        {
            hit = true;
            Direction(collider);
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
        GameObject.Find("Player").GetComponent<Player>().money += reward;
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
            GameObject.Find("Player").GetComponent<Player>().health -= health;
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
}
