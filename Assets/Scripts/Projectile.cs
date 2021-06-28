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

    public int burnTick;
    public int slowTick;

    private GameManager gameManager;
    private int reward;

    void Start()
    {
        if(transform.GetComponentInParent<JrollHandler>() == null)
        {
            StartCoroutine(DestroyProjectile());
        }
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.GetComponent<Enemy>().fire && !collider.GetComponent<Enemy>().ice)
        {
            CalculateReward(collider);

            if (GetComponentInParent<Tower>().doubleMoney)
            {
                DoubleMoney();
            }

            Debug.Log(reward);

            collider.GetComponent<Enemy>().reward = reward;
            RemoveEnemyHealth(collider);

            if (!slow)
            {
                gameManager.PlayPopNoise();
            }
            else
            {
                gameManager.PlayWhishNoise();
            }
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject);
        }
    }

    void RemoveEnemyHealth(Collider2D collider)
    {
        collider.GetComponent<Enemy>().health -= damage;
    }
    void CalculateReward(Collider2D collider)
    {
        if(damage > collider.GetComponent<Enemy>().health)
        {
            reward = collider.GetComponent<Enemy>().health;
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
