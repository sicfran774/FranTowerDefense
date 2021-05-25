using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JrollHandler : MonoBehaviour
{
    public EdgeCollider2D edge;
    public LayerMask layerMask;
    private TilemapCollider2D tileMap;

    private Vector2[] colliderpoints;
    private Vector2 direction;

    public float range;
    private float x;
    private float y;

    public bool hitPath;

    void Awake()
    {
        range = GetComponentInParent<Tower>().rangeRadius;
        tileMap = GameObject.Find("Tilemap").GetComponent<TilemapCollider2D>();

        colliderpoints = edge.points;
        colliderpoints[0] = Vector2.zero;

        hitPath = false;
    }

    
    void Update()
    {
        CheckIfLineIntersects();

        if (!hitPath)
        {
            GenerateRandomLine();
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

        int rand = Random.Range(0, 3);
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
}
