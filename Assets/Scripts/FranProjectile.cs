using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FranProjectile : MonoBehaviour
{
    public int damage;

    public bool burn;
    public int burnTick;

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
        UpdateEnemySpeed(collider);
        
        gameManager.GetComponent<GameManager>().PlayPopNoise(collider.gameObject);
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject);
    }
    
    void UpdateEnemySpeed(Collider2D collider)
    {
        collider.GetComponent<Enemy>().speed -= 0.5f;
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
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
