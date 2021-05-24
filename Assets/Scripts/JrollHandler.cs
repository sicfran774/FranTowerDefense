using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JrollHandler : MonoBehaviour
{
    public EdgeCollider2D edge;
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
        x = Random.Range(-range, range);
        MaintainHypotnuse();

        //Debug.Log("x: " + x + " y: " + y);
        direction = new Vector2(x, y);
        colliderpoints[1] = direction;

        edge.points = colliderpoints;
    }

    void MaintainHypotnuse()
    {
        y = Mathf.Sqrt(Mathf.Pow(range, 2) - Mathf.Pow(x, 2));

        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            x = -x;
        }
        else if (rand == 1)
        {
            y = -y;
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
}
