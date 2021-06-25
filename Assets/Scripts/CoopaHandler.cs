using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopaHandler : MonoBehaviour
{
    public GameObject target;

    public LayerMask layer;

    private GameObject towerTarget;
    private PlaceTower placeTower;
    private Tower tower;

    void Start()
    {
        towerTarget = Instantiate(target, GameObject.Find("Targets").transform);
        towerTarget.SetActive(false);

        placeTower = GetComponent<PlaceTower>();
        tower = GetComponent<Tower>();
    }
    void Update()
    {
        if (placeTower.placedTower && tag == "SelectedTower")
        {
            towerTarget.SetActive(true);
            tower.PointAtEnemy(ref towerTarget);
        }
        else
        {
            towerTarget.SetActive(false);
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layer);
            if (hit.collider != null)
            {
                hit.collider.gameObject.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            }
        }
    }
}
