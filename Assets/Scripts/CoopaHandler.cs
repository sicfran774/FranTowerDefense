using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopaHandler : MonoBehaviour
{
    [Header("Tower Stats")]
    public int damage;
    public int tickAmount;
    public float tickInterval;
    public float flameDuration;
    public float cooldownDuration;

    [Header("Misc")]
    public GameObject target;
    public LayerMask layer;

    private GameObject towerTarget;
    private PlaceTower placeTower;
    private Tower tower;
    private GameManager gameManager;

    private bool currentlyFiring;
    private bool cooldown;

    void Start()
    {
        towerTarget = Instantiate(target, GameObject.Find("Targets").transform);
        towerTarget.SetActive(false);

        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        transform.GetChild(2).gameObject.SetActive(false);
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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layer);

            if (hit.collider != null)
            {
                MoveTarget(hit, mousePosition);
            }
        }

        if(placeTower.placedTower && !currentlyFiring && !cooldown && gameManager.roundInProgress)
        {
            StartCoroutine(BeginFiring());
        }
    }
    IEnumerator BeginFiring()
    {
        currentlyFiring = true;

        ShowFlames();
        yield return new WaitForSeconds(flameDuration);
        HideFlames();

        cooldown = true;
        currentlyFiring = false;
        yield return new WaitForSeconds(cooldownDuration);
        cooldown = false;
    }

    void MoveTarget(RaycastHit2D hit, Vector3 mousePos)
    {
        hit.collider.gameObject.transform.position = new Vector2(mousePos.x, mousePos.y);
    }

    void ShowFlames()
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }
    void HideFlames()
    {
        transform.GetChild(2).gameObject.SetActive(false);
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Enemy" && currentlyFiring && !collider.GetComponent<Enemy>().flamed && !collider.GetComponent<Enemy>().fire)
        {
            collider.gameObject.GetComponent<Enemy>().burning = true;
            collider.gameObject.GetComponent<Enemy>().flamed = true;
            collider.gameObject.GetComponent<Enemy>().StartBurnTick(tickAmount, damage, tickInterval);
        }
    }
    void OnDestroy()
    {
        Destroy(towerTarget);
        StopAllCoroutines();
    }
}
