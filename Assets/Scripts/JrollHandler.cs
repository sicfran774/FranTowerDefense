using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JrollHandler : MonoBehaviour
{
    public EdgeCollider2D edge;
    public LayerMask layerMask;
    private TilemapCollider2D tileMap;
    private GameObject gameManager;

    private Vector2[] colliderpoints;
    private Vector2 direction;

    public float range;
    private float x;
    private float y;

    public bool hitPath;
    public bool activatedStack;

    void Awake()
    {
        range = GetComponentInParent<Tower>().rangeRadius;
        tileMap = GameObject.Find("Tilemap").GetComponent<TilemapCollider2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameController");

        colliderpoints = edge.points;
        colliderpoints[0] = Vector2.zero;

        hitPath = false;
        activatedStack = false;
    }

    
    void Update()
    {
        CheckIfLineIntersects();

        if (!hitPath)
        {
            GenerateRandomLine();
        }

        if (gameManager.GetComponent<GameManager>().roundInProgress)
        {
            if (GetComponent<Tower>().timer > GetComponent<Tower>().fireRate && GetComponent<PlaceTower>().placedTower && hitPath)
            {
                ApplyJroll();
            }
        }
    }

    public void GenerateRandomLine()
    {
        x = Random.Range(0, range);
        MaintainHypotnuse();

        //Debug.Log("x: " + x + " y: " + y);
        direction = new Vector2(x, y);
        colliderpoints[1] = direction;

        edge.points = colliderpoints;
    }

    void MaintainHypotnuse()
    {
        y = Mathf.Sqrt(Mathf.Pow(range, 2) - Mathf.Pow(x, 2));

        int rand = Random.Range(0, 4);
        if(rand == 0)
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
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    void CheckIfLineIntersects()
    {
        if (edge.IsTouching(tileMap))
        {
            hitPath = true;
        }
        else
        {
            hitPath = false;
        }
    }

    float GetDistanceFromPath()
    {
        RaycastHit2D fromCenterToEdge = Physics2D.Raycast(transform.position, direction, range, ~layerMask);

        if (fromCenterToEdge.collider != null)
        {
            float distance = range + (range - fromCenterToEdge.distance);

            return distance;
        }
        else
        {
            return range;
        }
    }

    public float GetLinearDrag()
    {
        //If you double the range, you calculate the drag needed to halve that distance. And so on and so on
        return GetDistanceFromPath();
    }

    public void ApplyJroll()
    {
        GameObject jrollSpike = GetComponent<Tower>().InstantiateAmmo(transform);
        jrollSpike.transform.position = transform.position;

        PushJroll(jrollSpike);

        if (activatedStack)
        {
            JrollStack(2);
        }

        GetComponent<JrollHandler>().GenerateRandomLine();
        GetComponent<Tower>().timer = 0;
    }

    void JrollStack(int numJrolls)
    {
        for (int count = 0; count < numJrolls; count++)
        {
            GameObject jrollSpike = GetComponent<Tower>().InstantiateAmmo(transform);
            jrollSpike.transform.position = new Vector2(transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y + Random.Range(-0.1f, 0.1f));
            PushJroll(jrollSpike);
        }
    }

    void PushJroll(GameObject jroll)
    {
        jroll.GetComponent<Rigidbody2D>().drag = GetComponent<JrollHandler>().GetLinearDrag();
        jroll.GetComponent<Rigidbody2D>().AddForce(GetComponent<JrollHandler>().GetDirection() * GetComponent<Tower>().projectileSpeed);
    }
}
