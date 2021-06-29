using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHandler : MonoBehaviour
{
    public bool stack;
    public int reward;

    public LayerMask layer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layer);

            if (hit.collider != null)
            {
                GameObject.Find("GameUI").GetComponent<Player>().money += reward;
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
