using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;

    [Header("Specific Properties")]
    public bool burn;
    public bool slow;

    public int burnTick;
    public int slowTick;

    private GameObject gameManager;
    private int reward;

    void Start()
    {
        if(transform.GetComponentInParent<JrollHandler>() == null)
        {
            StartCoroutine(DestroyProjectile());
        }
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        CalculateReward(collider);

        collider.GetComponent<Enemy>().reward = reward;
        RemoveEnemyHealth(collider);

        if (damage > 0)
        {
            gameManager.GetComponent<GameManager>().PlayPopNoise(collider.gameObject);
        }
        else
        {
            gameManager.GetComponent<GameManager>().PlayWhishNoise();
        }
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject);
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
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
