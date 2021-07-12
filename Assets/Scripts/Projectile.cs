using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int tickDamage;
    public float tickInterval = 1f;

    [Header("Specific Properties")]
    public bool burn;
    public bool slow;
    public bool canShootAllTypes;

    public int burnTick;
    public int slowTick;

    private GameManager gameManager;
    private int reward;

    void Start()
    {
        if(!transform.GetComponentInParent<Tower>().specialTower)
        {
            StartCoroutine(DestroyProjectile());
        }
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (canShootAllTypes || !enemy.fire && !enemy.ice && !enemy.boss)
        {
            HitEnemy(enemy);
            gameManager.PlayPopNoise();
        }
        else if (slow && enemy.fire)
        {
            HitEnemy(enemy);
            gameManager.PlayWhishNoise();
        }
        else if (burn && enemy.ice)
        {
            HitEnemy(enemy);
            gameManager.PlayPopNoise();
        }

        if (enemy.boss)
        {
            RemoveEnemyHealth(enemy);
            Destroy(gameObject);
            gameManager.PlayPopNoise();
        }
    }

    void HitEnemy(Enemy enemy)
    {
        CalculateReward(enemy);

        if (GetComponentInParent<Tower>().doubleMoney)
        {
            DoubleMoney();
        }

        enemy.reward = reward;
        RemoveEnemyHealth(enemy);
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject);
    }

    void RemoveEnemyHealth(Enemy enemy)
    {
        enemy.health -= damage;
    }
    void CalculateReward(Enemy enemy)
    {
        if(damage > enemy.health)
        {
            reward = enemy.health;
        }
        else
        {
            reward = damage;
        }

        if(reward < 1)
        {
            reward = 1;
        }
    }

    void DoubleMoney()
    {
        reward *= 2;
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
